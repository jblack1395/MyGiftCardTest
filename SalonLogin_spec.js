var frisby = require('./frisby');
frisby.create('SalonLogin')
  .post('http://localhost:8081/MyGiftCard/MyGiftCardService.svc/SalonLogin', {
      
          Salon: "testsalon",
          Username: "james",
          Password: "semaj"
  }, { json: true })
    .inspectBody()
  .expectStatus(202)
  .expectHeaderContains('content-type', 'application/json')
  .expectJSONTypes({token: String})
.toss();


var frisby = require('./frisby');
frisby.create('SalonLogin')
  .post('http://localhost:8081/MyGiftCard/MyGiftCardService.svc/SalonLogin', {

      Salon: "testsalon",
      Username: "james",
      Password: "james"
  }, { json: true })
  .expectStatus(401)
  .expectHeaderContains('content-type', 'application/json')
  .inspectBody()
.toss();


