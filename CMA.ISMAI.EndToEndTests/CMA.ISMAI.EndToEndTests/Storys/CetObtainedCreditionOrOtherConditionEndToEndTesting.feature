Feature: CetObtainedCreditionOrOtherConditionEndToEndTesting
	To make sure that this project works
	All processes will be tested in this feature.

Scenario: Test the solution behaivor in production
	When I navigate to Criar Processo page
	And I fill in the necessary form information  
	And Click on the submit button
	Then A sucess message should come up on the screen
	Then I need go to the course coordinators board on Trello
	And Put the created card on this board in done
	Then I need go to the course coordinators board on Trello again
	And Put the created card on this board in done, this card represent jury validation
	Then I need go to the cientific concil board on trello
	And Put the created card on cientific concil board in done