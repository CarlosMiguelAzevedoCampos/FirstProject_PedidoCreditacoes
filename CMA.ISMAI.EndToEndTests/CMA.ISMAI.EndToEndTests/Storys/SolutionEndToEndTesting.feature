Feature: SolutionEndToEndTesting
	To make sure that this project works
	All processes will be tested in this feature.

@mytag
Scenario: Test the solution behaivor in production
	When I navigate to Criar Processo
	And I fill in the necessary information 
	And Click on the submit button to crate the process
	Then A sucess message should appear on the screen
	Then I must go to the course coordinators board on Trello
	And Put the created card on the scientific council board in done
	Then I must go to the departament directors board on trello
	And Put the created card on the departament directors board in done
	Then I must go to the cientific concil board on trello
	And Put the created card on the cientific concil board in done