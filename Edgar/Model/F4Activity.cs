using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AiDollar.Edgar.Service.Model
{
    public class Issuer
    {
        public string issuerCik { get; set; }
        public string issuerName { get; set; }
        public string issuerTradingSymbol { get; set; }
    }

    public class ReportingOwnerId
    {
        public string rptOwnerCik { get; set; }
        public string rptOwnerName { get; set; }
    }

    public class ReportingOwnerAddress
    {
        public string rptOwnerStreet1 { get; set; }
        public string rptOwnerStreet2 { get; set; }
        public string rptOwnerCity { get; set; }
        public string rptOwnerState { get; set; }
        public string rptOwnerZipCode { get; set; }
        public string rptOwnerStateDescription { get; set; }
    }

    public class ReportingOwnerRelationship
    {
        public string isDirector { get; set; }
        public string isOfficer { get; set; }
        public string isTenPercentOwner { get; set; }
        public string isOther { get; set; }
        public string officerTitle { get; set; }
    }

    public class ReportingOwner
    {
        public ReportingOwnerId reportingOwnerId { get; set; }
        public ReportingOwnerAddress reportingOwnerAddress { get; set; }
        public ReportingOwnerRelationship reportingOwnerRelationship { get; set; }
    }

    public class SecurityTitle
    {
        public string value { get; set; }
    }

    public class TransactionDate
    {
        public string value { get; set; }
    }

    public class TransactionCoding
    {
        public string transactionFormType { get; set; }
        public string transactionCode { get; set; }
        public string equitySwapInvolved { get; set; }
    }

    public class TransactionTimeliness
    {
        public string value { get; set; }
    }

    public class FootnoteId
    {
        public string id { get; set; }
    }

    public class TransactionShares
    {
        public string value { get; set; }
        public FootnoteId footnoteId { get; set; }
    }

    public class TransactionPricePerShare
    {
        public string value { get; set; }
    }

    public class TransactionAcquiredDisposedCode
    {
        public string value { get; set; }
    }

    public class TransactionAmounts
    {
        public TransactionShares transactionShares { get; set; }
        public TransactionPricePerShare transactionPricePerShare { get; set; }
        public TransactionAcquiredDisposedCode transactionAcquiredDisposedCode { get; set; }
    }

    public class SharesOwnedFollowingTransaction
    {
        public string value { get; set; }
    }

    public class PostTransactionAmounts
    {
        public SharesOwnedFollowingTransaction sharesOwnedFollowingTransaction { get; set; }
    }

    public class DirectOrIndirectOwnership
    {
        public string value { get; set; }
    }

    public class OwnershipNature
    {
        public DirectOrIndirectOwnership directOrIndirectOwnership { get; set; }
    }

    public class NonDerivativeTransaction
    {
        public SecurityTitle securityTitle { get; set; }
        public TransactionDate transactionDate { get; set; }
        public TransactionCoding transactionCoding { get; set; }
        public TransactionTimeliness transactionTimeliness { get; set; }
        public TransactionAmounts transactionAmounts { get; set; }
        public PostTransactionAmounts postTransactionAmounts { get; set; }
        public OwnershipNature ownershipNature { get; set; }
    }

    public class SecurityTitle2
    {
        public string value { get; set; }
    }

    public class SharesOwnedFollowingTransaction2
    {
        public string value { get; set; }
    }

    public class PostTransactionAmounts2
    {
        public SharesOwnedFollowingTransaction2 sharesOwnedFollowingTransaction { get; set; }
    }

    public class DirectOrIndirectOwnership2
    {
        public string value { get; set; }
    }

    public class FootnoteId2
    {
        public string id { get; set; }
    }

    public class NatureOfOwnership
    {
        public string value { get; set; }
        public FootnoteId2 footnoteId { get; set; }
    }

    public class OwnershipNature2
    {
        public DirectOrIndirectOwnership2 directOrIndirectOwnership { get; set; }
        public NatureOfOwnership natureOfOwnership { get; set; }
    }

    public class NonDerivativeHolding
    {
        public SecurityTitle2 securityTitle { get; set; }
        public PostTransactionAmounts2 postTransactionAmounts { get; set; }
        public OwnershipNature2 ownershipNature { get; set; }
    }

    public class NonDerivativeTable
    {
        //public dynamic nonDerivativeTransactionList { get; set; }
        //public dynamic nonDerivativeHoldingList { get; set; }
        public dynamic nonDerivativeTransaction { get; set; }
        public dynamic nonDerivativeHolding { get; set; }

        public dynamic GetNonDerivativeTransaction()
        {
            return (nonDerivativeTransaction.GetType().Name ==  "JArray" ? nonDerivativeTransaction[0] : nonDerivativeTransaction);
            //return nonDerivativeTransactionList == null ? nonDerivativeTransaction : nonDerivativeTransactionList[0];
        }

        public dynamic GetNonDerivativeHolding()
        {
            return (nonDerivativeHolding.GetType().Name == "JArray" ? nonDerivativeHolding[0] : nonDerivativeHolding);
            //return nonDerivativeHoldingList == null ? nonDerivativeHolding : nonDerivativeHoldingList[0];
        }
    }

    public class Footnote
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Footnotes
    {
        public List<Footnote> footnote { get; set; }
    }

    public class OwnerSignature
    {
        public string signatureName { get; set; }
        public string signatureDate { get; set; }
    }

    public class OwnershipDocument
    {
        public string schemaVersion { get; set; }
        public string documentType { get; set; }
        public string periodOfReport { get; set; }
        public string notSubjectToSection16 { get; set; }
        public Issuer issuer { get; set; }
        public ReportingOwner reportingOwner { get; set; }
        public NonDerivativeTable nonDerivativeTable { get; set; }
        public dynamic footnotes { get; set; }
        public OwnerSignature ownerSignature { get; set; }
    }

    public class F4Activity
    {
        public OwnershipDocument ownershipDocument { get; set; }
    }

    public class InsideTrade
    {
        public string Issuer { get; set; }
        public string Symbol { get; set; }
        public string Cik { get; set; }
        public string Reporter { get; set; }
        public string Role { get; set; }
        public string TransactionCode { get; set; }
        public string Amount { get; set; }
        public string Price { get; set; }
        public string RemainAmount { get; set; }
        public string TransactionDate { get; set; }
    }


}
