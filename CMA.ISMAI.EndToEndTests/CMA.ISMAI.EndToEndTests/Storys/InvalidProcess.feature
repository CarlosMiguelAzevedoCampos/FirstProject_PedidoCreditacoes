Feature: Invalid Creditacao
	In order to avoid silly mistakes
	As a person
	I want to add a new process in the system but it should fail based on invalid parameters

@mytag
Scenario: Fail to add a new process
	When Navigate to Criar Novo Processo page
	When Entered the information in some fields
	When The submit button is clicked
	Then A label with the bad result should appear
