using AutoRepair.Data.Entities;
using AutoRepair.Models;

namespace AutoRepair.Helpers
{
    public interface IConverterHelper
    {
        Car ToCar(CarsViewModel model, bool isNew);
        Car ToCarCheck(Car model, bool isNew);
        Car ToCarUpdate(Car mode);
        CarsViewModel ToCarsViewModel(Car car);
        //public AutoPiece ToAutoPiece(AutoPiecesViewModel models, bool isNew);

        //public AutoPiecesViewModel ToAutoPieceViewModel(AutoPiece autoPiece);

    }
}
