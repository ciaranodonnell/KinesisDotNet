# A starter template / scaffolding for ApplicationService


*Key projects include*
**ApplicationService.API:**
*A Web API project for accepting requests*

**ApplicationService.DataAccess:**
*A class library for encapsulating DataAccess functionality*

**ApplicationService.EventProducer:**
*A class library for encapsulating Kinesis Producer functionality*

**ApplicationService.EventConsumer:**
*A class library for encapsulating Kinesis Consumer functionality using the Kinesis Client Library*

**ApplicationService.Tests:**
*A standard unit test project*

**NOTE:**
1. The Kinesis Consumer is still evolving. Currently the docker file launches the consumer as a console app. This behavior will be modified to launch the Web API and Consumer as a background process.
2. The API and the ApplicationService.EventConsumer can also be started on their own outside of the container to the get the messages flowing. Please make sure to change the kcl.properties file in EventConsumer project to set the correct stream.