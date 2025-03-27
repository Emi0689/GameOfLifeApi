This is an API that represents “The Game of Life - Conway”.

The API has 4 endpoints and works (swagger available)

1. POST /api/board 
    Description: Allows uploading a new board state, returns id of board
    Parameter: New Board to create (Example: [[1],[1]])
    Return: The id of new board created to be used in the others endpoints
2. PUT /api/{id}/next
    Description: Returns the new state of an existing board
    Parameter: the id of a board created to get the new state
    Return: 
3. PUT /api/{id}/next/{x}
    Description: Returns x number of states away for board
    Parameter: 
                id: the id of a board created to get the new state
                x: The number of generation to be executed
    Return: The board updated with "x" generations
4. PUT /api/{id}/final/{maxAttempts} 
    Description: Returns final state for board. If board doesn't go to conclusion after x number of attempts, returns error
    Parameter: 
            id: the id of a board created to get the new state
            maxAttempts: The number of generation to be executed for the final form
    Return: The board updated with "maxAttempts" generations
5. DELETE /api/{id}
    Description: Delete the board by Id
    Return: OK-200
    
Requirements:

You need to have .NET 7 SDK installed in the chosen server environment to work.