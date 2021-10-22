using AutoRepair.Data.Entities;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Microsoft.AspNetCore.Mvc;

namespace AutoRepair.Data
{
    public class InspecionRepository : GenericRepository<Inspecion>, IInspecionRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        public InspecionRepository(DataContext context, IUserHelper userHelper, IMailHelper mailHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        public IQueryable GetAllWithCars(int id)
        {
            var linqInspecion = from I in _context.Inspecions
                                join U in _context.Users
                                on I.User.Id equals U.Id
                                join C in _context.Cars
                                on U.Id equals C.User.Id
                                where C.Id == id
                                select new
                                {
                                    C.RegisterCar,
                                    I.InspecionDateStart,
                                    I.InspecionDate,
                                    I.InspecionHours,
                                    I.Price,
                                    I.Status
                                };

            return linqInspecion;
        }
        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var product = await _context.Cars.FindAsync(model.CarId);
            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await _context.InspecionDetailTemps
                .Where(odt => odt.User == user && odt.Car == product)
                .FirstOrDefaultAsync();

            if (orderDetailTemp == null)
            {
                orderDetailTemp = new InspecionDetailTemp
                {
                    Price = model.Price,
                    Car = product,
                    User = user
                };

                _context.InspecionDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                //orderDetailTemp.Quantity += model.Quantity;
                _context.InspecionDetailTemps.Update(orderDetailTemp);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }

            var orderTmps = await _context.InspecionDetailTemps
                .Include(o => o.Car)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new InspecionDetails
            {
                TotalPrice = o.Price,
                Car = o.Car,
            }).ToList();

            double price = 0;
            foreach (var item in details)
            {
                price = item.TotalPrice;
            }
            var order = new Inspecion
            {
                InspecionDateStart = DateTime.UtcNow,
                User = user,
                Items = details,
                Price = price,
                Status = "Pending",
                Username = user.UserName
            };

            await CreateAsync(order);
            _context.InspecionDetailTemps.RemoveRange(orderTmps);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await _context.InspecionDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            _context.InspecionDetailTemps.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task DeliverOrder(MarkViewModel model)
        {
            var order = await _context.Inspecions.FindAsync(model.Id);

            if (order == null)
            {
                return;
            }

            if (order.Status == "In Processing")
            {
                order.Status = "Completed";
                _context.Inspecions.Update(order);
                await _context.SaveChangesAsync();

                return;
            }
            if (order.Status == "Completed")
            {


                //Create a new PDF document
                PdfDocument document = new PdfDocument();

                //Add a page to the document
                PdfPage page = document.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;

                //Set the standard font
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                //Draw the text
                graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));

                //Saving the PDF to the MemoryStream
                MemoryStream stream = new MemoryStream();

                document.Save(stream);

                //Set the position as '0'.
                stream.Position = 0;

                //Download the PDF document in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

                fileStreamResult.FileDownloadName = "aaaa.pdf";

                order.Status = "Finished";
                _context.Inspecions.Update(order);
                await _context.SaveChangesAsync();

                var a = _context.Inspecions;
                _mailHelper.SendEmail(order.Username, "Your Invoice", "Your Details:" +
                    $"\n\n Inspecion Date:{order.InspecionDate}   Hours:{order.InspecionHours}" +
                    $"\n\n Price:{order.Price}" +
                    $"\n\n\n\n<h3>Thanks for choosing us!!! :)</h3>");



                //Creates a FileContentResult object by using the file contents, content type, and file name.

                // FileContentResult(stream, contentType, fileName);

                return;
            }

            order.InspecionDate = model.DeliveryDate;
            order.InspecionHours = model.InspecionHours;
            order.Status = "In Processing";
            _context.Inspecions.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<InspecionDetailTemp>> GetDetailTempsAsync(string userName)
        {

            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return _context.InspecionDetailTemps
                .Include(p => p.Car)
                .Where(o => o.User == user)
                .OrderBy(o => o.Car.RegisterCar);
        }

        public async Task<IQueryable<Inspecion>> GetOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Inspecions
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(p => p.Car)
                    .OrderByDescending(o => o.InspecionDateStart);
            }

            return _context.Inspecions
                .Include(o => o.Items)
                .ThenInclude(p => p.Car)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.InspecionDateStart);
        }


        public async Task<Inspecion> GetOrderAsync(int id)
        {
            return await _context.Inspecions.FindAsync(id);
        }
        public IQueryable<Inspecion> GetOrderInProcessing()
        {

            return _context.Inspecions
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(p => p.Car)
                .Where(o => o.Status == "In Processing")
                .OrderByDescending(o => o.InspecionDateStart);
        }
        public IQueryable<Inspecion> GetOrderCompleted()
        {

            return _context.Inspecions
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(p => p.Car)
                .Where(o => o.Status == "Completed" || o.Status == "Pending" || o.Status == "Finished")
                .OrderByDescending(o => o.InspecionDateStart);
        }
        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await _context.InspecionDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            //orderDetailTemp.Quantity += quantity;
            //if (orderDetailTemp.Quantity > 0)
            //{
            //    _context.OrderDetailsTemp.Update(orderDetailTemp);
            //    await _context.SaveChangesAsync();
            //}
        }

    }
}
