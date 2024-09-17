namespace API.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required AddressDto Address { get; set; }
    }
}


// Sample Payloads for customer

//{
//  "name": "John Doe",
//  "email": "john.doe@example.com",
//  "phoneNumber": "+1-555-1234",
//  "address":{
//        "address1": "123 Elm St",
//        "address2": "Apt 4B",
//        "city": "New York",
//        "country": "USA",
//        "state": "NY",
//        "zipCode": "10001"
//  }
//}

//{
//   "name": "Jane Smith",
//  "email": "jane.smith@example.com",
//  "phoneNumber": "+44-20-7946-0958",
//  "address": {
//        "address1": "456 Oak St",
//    "address2": "Suite 501",
//    "city": "London",
//    "country": "UK",
//    "state": "London",
//    "zipCode": "SW1A 1AA"
//  }
//}

//{
//  "name": "Liu Wei",
//  "email": "liu.wei@example.com",
//  "phoneNumber": "+86-10-1234-5678",
//  "address": {
//        "address1": "789 Pine Rd",
//    "address2": "Building 3",
//    "city": "Beijing",
//    "country": "China",
//    "state": "Beijing",
//    "zipCode": "100000"
//  }
//}

//{
//    "name": "Maria Garcia",
//  "email": "maria.garcia@example.com",
//  "phoneNumber": "+34-91-123-4567",
//  "address": {
//        "address1": "321 Maple Ave",
//    "address2": "",
//    "city": "Madrid",
//    "country": "Spain",
//    "state": "Madrid",
//    "zipCode": "28001"
//  }
//}

