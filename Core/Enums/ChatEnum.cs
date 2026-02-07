using System;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ParticipantType
    {
        Admin,
        Provider,
        Client
    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SenderType
    {
        Admin,
        Provider,
        Client
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MentionedType
    {
        Admin,
        Provider,
        Client
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageType
    {
        Note,
        ManualNote,
        Message,
        IMAGE,
        VIDEO,
        AUDIO,
        FILE,
        LOCATION,
        STICKER,
        SYSTEM,
        REPLY,
        DELETED
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageContentType
    {
        TEXT,
        IMAGE,
        VIDEO,
        AUDIO,
        FILE,
        LOCATION,
        STICKER,
        SYSTEM,
        REPLY,
        DELETED
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageNoteType
    {
        AddToShoppingCardByClient,
        AddToShoppingCardByAdmin,

        DeleteFromShoppingCardByClient,
        DeleteFromShoppingCardByAdmin,

        ContractCreate,
        ContractSend,

        ContractSignedByClient,
        ContractSignedByAdmin,

        RequestCreateByClient,
        RequestCreateByAdmin,

        RequestCanceledByClient,
        RequestCanceledByAdmin,

        RequestUpdatedbyClient,
        RequestUpdatedbyAdmin,

        ManifestAssigned,

        SendToClientForPayment,

        PayedByAdmin,
        PayedByClient,
    }
    public static class MessageNoteTypeExtensions
    {
        public static string Description(this MessageNoteType type)
        {
            return type switch
            {
                MessageNoteType.AddToShoppingCardByClient => "Item Added to Cart",
                MessageNoteType.AddToShoppingCardByAdmin => "Item Added to Cart",

                MessageNoteType.DeleteFromShoppingCardByClient => "Item Removed from Cart",
                MessageNoteType.DeleteFromShoppingCardByAdmin => "Item Removed from Cart",

                MessageNoteType.ContractSend => "Contract Sent",
                MessageNoteType.ContractCreate => "Contract Created",
                MessageNoteType.ContractSignedByClient => "Contract Signed",
                MessageNoteType.ContractSignedByAdmin => "Contract Signed",

                MessageNoteType.RequestCreateByClient => "Request Created",
                MessageNoteType.RequestCreateByAdmin => "Request Created",

                MessageNoteType.RequestCanceledByClient => "Request Canceled",
                MessageNoteType.RequestCanceledByAdmin => "Request Canceled",

                MessageNoteType.RequestUpdatedbyClient => "Request Updated",
                MessageNoteType.RequestUpdatedbyAdmin => "Request Updated",

                MessageNoteType.ManifestAssigned => "Manifest Assigned",
                MessageNoteType.SendToClientForPayment => "Sent for Payment",

                MessageNoteType.PayedByAdmin => "Payment Completed",
                MessageNoteType.PayedByClient => "Payment Completed",

                _ => "System Note",
            };
        }
    }
}