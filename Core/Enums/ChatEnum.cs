using System;
using System.Text.Json.Serialization;

namespace CMPNatural.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ParticipantType
    {
        Admin,
        Provider,
        Driver,
        Client
    }


    //[JsonConverter(typeof(JsonStringEnumConverter))]
    //public enum SenderType
    //{
    //    Admin,
    //    Provider,
    //    Driver,
    //    Client
    //}

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

        RequestReActivateByAdmin,

        RequestUpdatedbyClient,
        RequestUpdatedbyAdmin,

        ContractDeletedbyAdmin,

        ManifestScaduled,
        ManifestCreated,
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
                MessageNoteType.ContractSignedByClient => "Contract Signed By Client",
                MessageNoteType.ContractSignedByAdmin => "Contract Signed By Admin",

                MessageNoteType.RequestCreateByClient => "Request Created",
                MessageNoteType.RequestCreateByAdmin => "Request Created",

                MessageNoteType.RequestCanceledByClient => "Request Canceled",
                MessageNoteType.RequestCanceledByAdmin => "Request Canceled",

                MessageNoteType.RequestReActivateByAdmin => "Request Activated",


                MessageNoteType.RequestUpdatedbyClient => "Request Updated",
                MessageNoteType.RequestUpdatedbyAdmin => "Request Updated And Contract Created",

                MessageNoteType.ContractDeletedbyAdmin => "Contract Deleted",

                MessageNoteType.ManifestCreated => "Manifest Created",
                MessageNoteType.ManifestScaduled => "Manifest Scaduled",


                MessageNoteType.ManifestAssigned => "Manifest Assigned",
                MessageNoteType.SendToClientForPayment => "Sent for Payment",

                MessageNoteType.PayedByAdmin => "Payment Completed",
                MessageNoteType.PayedByClient => "Payment Completed",

                _ => "System Note",
            };
        }
    }
}