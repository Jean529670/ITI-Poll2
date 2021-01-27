Feature: Vote

Scenario Outline: A guest vote for a proposal
  Given a poll with some proposals
  When user with <userId> vote for the proposal <n>
  Then his vote should be the proposal <n>
  And the voter should be the user with <userId>

  Examples:
    | userId | n |
    |      2 | 1 |
    |      3 | 2 |
    |      4 | 3 |
