var frisby = require('./frisby');
frisby.create('GetSalonList')
  .get('http://localhost:8081/MyGiftCard/MyGiftCardService.svc/GetSalonList/')
  .expectStatus(200)
  .expectHeaderContains('content-type', 'application/json')
  .expectJSON('0', {
      SalonName: function (val) { expect(val).toMatchOrBeNull("Name 1"); }, // Custom matcher callback
      SalonAddress: {
          AddressOne: "address one",
          State: "TN"
      }
  })
  .expectJSONTypes('0', {
      AmericanExpressAccepted: Boolean,
      SalonName: function (val) { expect(val).toBeTypeOrNull(String); }, // Custom matcher callback
      SalonAddress: {
          AddressOne: String,
          State: String
      }
  })
.toss();