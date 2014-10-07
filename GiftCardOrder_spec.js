var frisby = require('./frisby');
frisby.create('GiftCardOrder')
  .post('http://localhost:8081/MyGiftCard/MyGiftCardService.svc/GiftCardOrder', {
      CardInfo: {
          CardType: "Mastercard",
          CardNumber: "3456545653345677",
          ExpirationDate: "8/2015"
      },
      Message: "Just for giggles",
      Email: "fakeemail"
  }, { json: true })
  .expectStatus(200)
  .expectHeaderContains('content-type', 'application/json')
  .inspectBody()
.toss();


