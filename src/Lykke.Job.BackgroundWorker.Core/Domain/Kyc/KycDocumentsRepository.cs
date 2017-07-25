using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.Kyc
{
    public static class DocumentStates
    {
        public const string Uploaded = "Uploaded";
        public const string Approved = "Approved";
        public const string Declined = "Declined";
    }

    public interface IKycDocument
    {
        string ClientId { get; }
        string DocumentId { get; }
        string Type { get; }
        string Mime { get; }
        string KycComment { get; }
        string State { get; }

        string FileName { get; }
        DateTime DateTime { get; }
        string DocumentName { get; }
    }


    public class KycDocument : IKycDocument
    {
        public string ClientId { get; set; }
        public string DocumentId { get; set; }
        public string Type { get; set; }
        public string Mime { get; set; }
        public string KycComment { get; set; }
        public string State { get; set; }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }
        public string DocumentName { get; set; }

        public static KycDocument Create(string clientId, KycDocumentTypeApi type, string mime, string fileName)
        {
            return new KycDocument
            {
                ClientId = clientId,
                Type = type.ToText(),
                Mime = mime,
                DateTime = DateTime.UtcNow,
                FileName = fileName,
                State = DocumentStates.Uploaded
            };
        }

        public static KycDocument Create(IKycDocument src)
        {
            return new KycDocument
            {
                ClientId = src.ClientId,
                Type = src.Type,
                Mime = src.Mime,
                DateTime = DateTime.UtcNow,
                FileName = src.FileName,
                DocumentId = src.DocumentId,
                KycComment = src.KycComment,
                State = src.State
            };
        }
    }

    public interface IKycDocumentsRepository
    {
        Task AddAsync(IEnumerable<IKycDocument> kycDocuments);
        Task<IKycDocument> AddAsync(IKycDocument kycDocument);
        Task<IEnumerable<IKycDocument>> GetAsync(string clientId);
        Task<IKycDocument> GetAsync(string clientId, string documentId);
        Task<IEnumerable<IKycDocument>> GetOneEachTypeLatestAsync(string clientId, bool showDeclined = false);
        Task<IEnumerable<IKycDocument>> GetKycSpiderDocuments(string clientId, bool showDeclined = false);
        Task<IKycDocument> DeleteAsync(string clientId, string documentId);
        Task<IKycDocument> SetStateAndComment(string clientId, string documentId, string comment, string state);
        Task SetLatestApproved(string clientId);
    }


    public enum KycDocumentTypeApi
    {
        Unknown, IdCard, ProofOfAddress, Selfie
    }

    public static class KycDocumentTypes
    {
        public static IEnumerable<KycDocumentTypeApi> GetAllTypes()
        {
            yield return KycDocumentTypeApi.IdCard;
            yield return KycDocumentTypeApi.ProofOfAddress;
            yield return KycDocumentTypeApi.Selfie;
        }

        public static string GetDocumentTypeName(KycDocumentTypeApi type)
        {
            switch (type)
            {
                case KycDocumentTypeApi.IdCard:
                    return "Passport or ID";
                case KycDocumentTypeApi.ProofOfAddress:
                    return "Proof of address";
                case KycDocumentTypeApi.Selfie:
                    return "Selfie";
                default:
                    return "Unknown";
            }
        }

        public static string ToText(this KycDocumentTypeApi type)
        {
            switch (type)
            {
                case KycDocumentTypeApi.IdCard: return "IdCard";
                case KycDocumentTypeApi.ProofOfAddress: return "ProofOfAddress";
                case KycDocumentTypeApi.Selfie: return "Selfie";
                default:
                    return string.Empty;

            }
        }

        public static bool HasType(this IEnumerable<IKycDocument> documents, KycDocumentTypeApi type)
        {
            var strType = type.ToText();
            var doc = documents.FirstOrDefault(itm => itm.Type == strType);
            return doc != null && doc.State != DocumentStates.Declined;
        }


        public static bool HasAllTypes(this IEnumerable<IKycDocument> documents)
        {
            var docs = documents as IKycDocument[] ?? documents.ToArray();

            var idCard = docs.FirstOrDefault(itm => itm.Type == KycDocumentTypeApi.IdCard.ToText());
            var proof = docs.FirstOrDefault(itm => itm.Type == KycDocumentTypeApi.ProofOfAddress.ToText());
            var selfie = docs.FirstOrDefault(itm => itm.Type == KycDocumentTypeApi.Selfie.ToText());

            return idCard != null && idCard.State != DocumentStates.Declined
                   && proof != null && proof.State != DocumentStates.Declined
                   && selfie != null && selfie.State != DocumentStates.Declined;
        }

        public static KycDocumentTypeApi ToKycDocumentTypeApi(this string typeApi)
        {
            KycDocumentTypeApi kycDocType;
            Enum.TryParse(typeApi, out kycDocType);

            return kycDocType;
        }

        public static bool IsKycDocumentType(this string typeApi)
        {
            KycDocumentTypeApi kycDocType;
            Enum.TryParse(typeApi, out kycDocType);

            return kycDocType != KycDocumentTypeApi.Unknown;
        }

        public static bool IsKycSpiderDocumentType(this string typeApi)
        {
            return typeApi == KycSpiderDocument.DocumentTypeRepositoryText;
        }
    }

    public class KycDocumentOther : IKycDocument
    {
        public string ClientId { get; set; }
        public string DocumentId { get; set; }

        public string Type
        {
            get { return $"{KycSpiderDocument.DocumentTypeRepositoryText}: \"{DocumentName}\""; }
            set { }
        }

        public string Mime { get; set; }
        public string KycComment { get; set; }
        public string State { get; set; }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }
        public string DocumentName { get; set; }
    }

    public class KycSpiderDocument : IKycDocument
    {
        public static string DocumentTypeUiText => "Other";
        public static string DocumentTypeRepositoryText => "Other";

        public KycSpiderDocument(string clientId, string mime, string state, string fileName, string documentName)
        {
            ClientId = clientId;
            Mime = mime;
            State = state;
            FileName = fileName;
            DateTime = DateTime.UtcNow;
            DocumentName = documentName;
        }

        public string ClientId { get; set; }
        public string DocumentId { get; set; }

        public string Type
        {
            get { return KycSpiderDocument.DocumentTypeRepositoryText; }
            set { }
        }

        public string Mime { get; set; }
        public string KycComment { get; set; }
        public string State { get; set; }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }
        public string DocumentName { get; set; }
    }

}