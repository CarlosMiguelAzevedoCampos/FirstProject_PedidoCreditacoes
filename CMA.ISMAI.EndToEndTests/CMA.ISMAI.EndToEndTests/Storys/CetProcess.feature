Feature: CreditacaoCet
	In order to avoid silly mistakes
	As a person
	I want to add a new process in the system

@mytag
Scenario: Add a new process for a cet process
	When I navigate to Criar Novo Processo
	When Entered the form asking details
	When The submit button is pressed
	Then The result of the operation should appear in the screen
