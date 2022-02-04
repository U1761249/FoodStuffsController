# FoodStuffsController
Assignment for the Advanced Software Development module.


Foodstuffs Production Scenario

There are three feed bins in the production bay. Bins are identified by a unique bin number. 
A bin can contain any product, but only one product at a time.  The quantity of product in a 
bin is measured in cubic metres.  Any product removed from a bin is discharged onto a 
conveyor belt which feeds the mixing machine. Bins have a maximum capacity of 40 cubic 
metres.

A recipe includes a percentage of each of two products, e.g. 

Recipe 1 FlakyMeal 
30%  Cornmeal   70%  Crushed Flakes 

Recipe 2 WeetyMeal 
40%  Weety Bits  60%  Bitty Weets 

A batch is a specified volume of a product, e.g. 
20 cubic metres of recipe 2 
(given the recipe above, this would include 8 cubic metres of Weety Bits and 12 cubic 
metres of Bitty Weets being removed from the appropriate bins) 

A day's production schedule includes a list of batches to be made up that day. 
At the beginning of the day an initial volume of product is loaded in to the bins to enable 
production of the first few batches, then the bins are replenished as needed later in the day 
to complete the day's production. 
The bin controller is responsible for maintaining an appropriate volume of product in each 

bin, following verbal orders given by the production supervisor. 
The production supervisor is responsible for making up product batches and checking that 
the next batch can be made up, given the current state of the bins. 
