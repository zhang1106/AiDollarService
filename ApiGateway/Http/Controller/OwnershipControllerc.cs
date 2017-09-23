using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Bam.Oms.Compliance;
using Bam.Oms.Compliance.Services;
using Bam.Oms.Data.Compliance;
using Bam.Oms.Data.Enumerators;
using Bam.Oms.Data.Positions;

namespace Bam.Compliance.ApiGateway.Http.Controller
{
    public class OwnershipController : ApiController
    {
        private readonly IHeadRoomCalculator _headRoomCalculator;
        private readonly IPositionSvc _positionSvc;
        private readonly IFirmPositionComplianceSvc _complianceSvc;

        public OwnershipController(IHeadRoomCalculator headRoomCalculator, IPositionSvc positionSvc,
            IFirmPositionComplianceSvc complianceSvc)
        {
            _headRoomCalculator = headRoomCalculator;
            _positionSvc = positionSvc;
            _complianceSvc = complianceSvc;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IList<HeadRoom> GetNewHeadRooms(HeadRommRequest req)
        {
            return _headRoomCalculator.GetNewHeadRooms(req.symbols, false).ToList();
        }

        [AllowAnonymous]
        public string GetSodHeadRooms()
        {
            try
            {
                _complianceSvc.Run(PositionType.Sod);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Succeed";
        }

        [AllowAnonymous]
        public string GetIntradayReport()
        {
            try
            {
                _complianceSvc.Run(PositionType.Intraday);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Succeed";
        }

        [AllowAnonymous]
        public IList<FlatPosition> GetDecomposedPostionsByIsin(string isin, PositionType type, bool dumpToDb = false)
        {
            var decomposed = _positionSvc.GetDecomposedPositionsByIsin(isin, type, dumpToDb);

            return decomposed?.ToList();
        }

        [AllowAnonymous]
        public IList<FlatPosition> GetPositionsByUnderlying(PositionType type, string symbol)
        {
            var positions = _positionSvc.GetPositionsByUnderlying(type, symbol);
            return positions?.ToList();
        }

        [AllowAnonymous]
        public IList<Constituent> GetConstituents(string symbol)
        {
            var constituents = _positionSvc.GetConstituents(symbol);
            return constituents?.ToList();
        }

        [AllowAnonymous]
        public string GetEodReport(DateTime date)
        {
            try
            {
                _complianceSvc.RunEod(date.Date);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Succeed";
        }

        [AllowAnonymous]
        [HttpGet]
        public IList<HeadRoom> CheckEodPosition(string symbol)
        {
            try
            {
              var result = _headRoomCalculator.CheckEodPosition(symbol);
                return result.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        [AllowAnonymous]
        public string RefreshRefData()
        {
            try
            {
                _headRoomCalculator.RefreshRefData();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Succeed";
        }
        
        public struct HeadRommRequest
        {
            public string[] symbols { get; set; }
            public bool sod { get; set; }
        }
    }
}
