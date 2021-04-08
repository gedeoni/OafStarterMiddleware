@smoke @atm
Feature: Create Worlds

    Scenario Outline: Creating a World where it doesn't exist
        Given that <World> does not exist in the database
        When a call is made to <CreateWorld>
        Then a <World> must be returned
        And and a <world-created> event must be published to the <EventBus>
        And that <World> must be available in the database