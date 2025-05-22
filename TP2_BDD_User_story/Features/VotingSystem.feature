Feature: VotingSystem
	
# (i) Ballot = scrutin
	
# Victoire au premier tour avec 50%
Scenario: A candidate exceeds 50% on first round
	Given a ballot starts
	And the votes are : 
	  | Candidate | Votes |
	  | Alice     | 60    |
	  | Bob       | 30    |
	  | Charlie   | 10    |
   	Then the ballot is on first round
   	Given the ballot has ended
	When we calculate the ballot results
	Then the winner is Alice
	And the results are :
	  | Candidate | Votes | Percentage |
	  | Alice     | 60    | 60         |
	  | Bob       | 30    | 30         |
	  | Charlie   | 10    | 10         |
   
# Test des votes blanc
Scenario: A candidate exceeds 50% on first round (with blank votes)
	Given a ballot starts
	And the votes are : 
	  | Candidate | Votes |
	  | Alice     | 55    |
	  | Bob       | 25    |
	  | Charlie   | 5     |
   	And 15 blank votes are submitted
	Then the ballot is on first round
	Given the ballot has ended
	When we calculate the ballot results
	Then the winner is Alice
	And the results are :
	  | Candidate | Votes | Percentage |
	  | Alice     | 55    | 55         |
	  | Bob       | 25    | 25         |
	  | Charlie   | 5     | 5          |
	  | Blank     | 15    | 15         |
 
# On ne peut pas avoir les résultats si le scrutin n'est pas clôturé
Scenario: Trying to get results before closing the ballot
	Given a ballot starts
	And the votes are : 
	  | Candidate | Votes |
	  | Alice     | 45    |
	  | Bob       | 30    |
	  | Charlie   | 10    |
	When we calculate the ballot results
	Then the results are not given
	
# Victoire au second tour 
Scenario: A candidate wins on second round
	Given a ballot starts
	And the votes are : 
	  | Candidate | Votes | 
	  | Alice     | 25    | 
	  | Bob       | 30    | 
	  | Charlie   | 10    | 
	  | Foxtrot   | 15    | 
	  | Delta     | 20    |
	Then the ballot is on first round
	Given the ballot has ended
	When we calculate the ballot results
	# Pas de gagnant car pas au dessus de 50%
	Then there is no winner 
	And the results are :
	  | Candidate | Votes | Percentage |
	  | Bob       | 30    | 30         |
	  | Alice     | 25    | 25         |
	  | Delta     | 20    | 20         |
	  | Foxtrot   | 15    | 15         |
	  | Charlie   | 10    | 10         |
	And the ballot is on second round
	Given the votes are : 
	  | Candidate | Votes |
	  | Alice     | 30    |
	  | Bob       | 70    |
   	And the ballot has ended
	When we calculate the ballot results
	Then the winner is Bob
	And the results are :
	  | Candidate | Votes | Percentage |
	  | Alice     | 30    | 30         |
	  | Bob       | 70    | 70         |
   	And no more round starts
   
# Égalité au second tour (pas de majorité)
Scenario: Equal results on second round
	Given a ballot starts
	And the votes are : 
	  | Candidate | Votes | 
	  | Alice     | 25    | 
	  | Bob       | 30    | 
	  | Charlie   | 10    | 
	  | Foxtrot   | 15    | 
	  | Delta     | 20    |
	Then the ballot is on first round
	Given the ballot has ended
	When we calculate the ballot results
	Then there is no winner
	And the results are :
	  | Candidate | Votes | Percentage |
	  | Bob       | 30    | 30         |
	  | Alice     | 25    | 25         |
	  | Delta     | 20    | 20         |
	  | Foxtrot   | 15    | 15         |
	  | Charlie   | 10    | 10         |
	And the ballot is on second round
	Given the votes are : 
	  | Candidate | Votes |
	  | Alice     | 50    |
	  | Bob       | 50    |
	And the ballot has ended
	When we calculate the ballot results
	Then there is no winner
	And the results are :
	  | Candidate | Votes | Percentage |
	  | Alice     | 50    | 50         |
	  | Bob       | 50    | 50         |
    # On ne lance pas un troisième tour car 2 tours maximum
	And no more round starts 
	
# Le 2e et le 3e sont ex aequo au premier tour -> les deux sont qualifiés au second tour
Scenario: Equal results for second and third candidate on first round
	Given a ballot starts
	And the votes are : 
	  | Candidate | Votes | 
	  | Alice     | 25    | 
	  | Bob       | 30    | 
	  | Charlie   | 10    | 
	  | Foxtrot   | 10    | 
	  | Delta     | 25    |
	Then the ballot is on first round
	Given the ballot has ended 
	When we calculate the ballot results
	Then there is no winner
	And the results are :
	  | Candidate | Votes | Percentage |
	  | Bob       | 30    | 30         |
	  | Alice     | 25    | 25         |
	  | Delta     | 25    | 25         |
	  | Foxtrot   | 10    | 10         |
	  | Charlie   | 10    | 10         |
	And the ballot is on second round
	And the three candidates are qualified