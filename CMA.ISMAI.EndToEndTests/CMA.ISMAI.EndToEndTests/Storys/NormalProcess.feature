Feature: Creditacao
	In order to avoid silly mistakes
	As a person
	I want to add a new process in the system

@mytag
Scenario: Add a new process for a normal process
	When Navigate to Criar Novo Processo
	When Enter the form asking details
	When Click on the submit button
	Then The result should appear in the screen
