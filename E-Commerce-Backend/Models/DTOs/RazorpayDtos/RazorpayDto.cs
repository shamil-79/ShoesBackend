namespace E_Commerce_Backend.Models.DTOs.RazorpayDtos
{
    public class RazorpayDto
    {
        public string razorpay_payment_id { get; set; }

        public string razorpay_order_id { get; set; }

        public string razorpay_signature { get; set; }
    }
}
