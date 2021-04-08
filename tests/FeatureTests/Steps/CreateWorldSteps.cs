using FluentAssertions;
using TechTalk.SpecFlow;

namespace FeatureTests.Steps
{
    [Binding]
    public class CreateWorldSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public CreateWorldSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }


        [Given(@"that <World> does not exist in the database")]
        public void GivenWorldDoesNotExistInDatabase()
        {
            //_worldRepo.FindAllDocuments().Should().BeEmpty();
        }

        [When(@"a call is made to <CreateWorld>")]
        public void WhenCreateWorldIsCalled()
        {
            //_fakeWorld = dto;
            //_createdWorld = _mediator.SendAsync(new CreateWorldCommand(_fakeWorld));
        }

        [Then(@"a <World> must be returned")]
        public void WorldMustBeCreated()
        {
            //_fakeWorld.Name === _createdWorld.Name
        }

        [Then(@"and a <world-created> event must be published to the <EventBus>")]
        public void EventMustBePublished()
        {
            //is there a way to hook into Rabbit? or mock publishEvent in EventHandler
        }

        [Then(@"that <World> must be available in the database")]
        public void WorldMustBeAvailableInDatabase()
        {
            //_worldRepo.FindOne(_createdWorld.Id);
        }
    }
}