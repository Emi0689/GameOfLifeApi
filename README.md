This is an API that represents “The Game of Life - Conway”.

The API has 4 endpoints and works (swagger available)

1. POST /api/board  
    i. Description: Allows uploading a new board state, returns id of board  
    ii. Parameter: New Board to create (Example: [[1],[1]])  
    iii. Return: The id of new board created to be used in the others endpoints  
2. PUT /api/{id}/next  
    i. Description: Returns the new state of an existing board  
    ii. Parameters:  
        -    id: the id of a board created to get the new state  
    iii. Return: The board updated  
3. PUT /api/{id}/next/{x}  
    i. Description: Returns x number of states away for board  
    ii. Parameters:  
        -    id: the id of a board created to get the new state  
        -    x: The number of generation to be executed  
    iii. Return: The board updated with "x" generations  
4. PUT /api/{id}/final/{maxAttempts}  
    i. Description: Returns final state for board. If board doesn't go to conclusion after x number of attempts, returns error  
    ii. Parameters:  
        -    id: the id of a board created to get the new state  
        -    maxAttempts: The number of generation to be executed for the final form  
    iii. Return: The board updated with "maxAttempts" generations  
5. DELETE /api/{id}  
    i. Description: Delete the board by Id  
    ii. Parameters:  
        -    id: the id of a board created to get the new state  
    iii. Return: OK-200  
    
Requirements:

It use memory and SQLite to work. So BEFORE to start:

1. You need to have .NET 7 SDK installed in the chosen server environment to work.

2. In a new terminal, inside of the folder "GameOfLifeApi" you need to run these commands:  

 - dotnet ef migrations add InitialCreate  
 - dotnet ef database update  

To create the SQLite DB.  