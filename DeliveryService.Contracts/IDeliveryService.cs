using ProtoBuf;
using ProtoBuf.Grpc;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace DeliveryService.Contracts
{
    // dotnet add package protobuf-net.Grpc

    [ServiceContract]
    public interface IDeliveryService
    {
        // [OperationContract]
        Task<DeliveryResponse> ConfirmDeliveryAsync(DeliveryRequest request, CallContext context = default);
    }


    //[DataContract]
    [ProtoContract]
    public class DeliveryRequest
    {
        // [DataMember(Order = 1)]
        [ProtoMember(1)]
        public int DeliveryId { get; set; }

        // [DataMember(Order = 2)]
        [ProtoMember(2)]
        public DateTime DeliveryDate { get; set; }

        // [DataMember(Order = 3)]
        [ProtoMember(3)]
        public string Sign { get; set; }
    }

    // [DataContract]
    [ProtoContract]
    public class DeliveryResponse
    {
        //[DataMember(Order = 1)]
        [ProtoMember(1)]
        public bool Confirmed { get; set; }
    }
}