Factory

This is simple tool which allows a user to manage simplified auto maker tasks from manufacturing to selling of vehicles (i.e. cars & boats).

Features of this tool include:
- Management of units within a category (Creating, Editing, Deleting, Test driving, and Selling)
- Vehicle specifications which allow for detailed configuration of units
- Tracking of total profits and units sold
- Tracking of inventory value and number of units within a category

Technologies used:
- Entity Framework Core for object-database mapping and interaction with the database.
- SQLite database engine for quick and easy development. (Not practical at larger scale).

Practices followed:
- Classes are created in a way which supports Entity Framework object-database mapping.
- Both 'Car' and 'Boat' are inherited from the abstract class 'Vehicle'.
- Polymorphism is used as a means to change the test driving message, based on which vehicle is calling it.
- Static utility classes exist for Wpf and Vehicle related tasks, which helps consolidate code and avoid repetition.
  This makes the code easier to read and ensures that any future changes will only need to be made in one place.
- Error handling is implemented for both user inputs and anytime the database is accessed.
  Try/Catches surround every instance of database access and provide a user friendly error message, 
  as well as the exception itself in the form of a error popup.

Potential improvements:
- Make the WPF forms populate data dynamically based on which vehicle type is selected (boat, car) to reduce ammount of needed windows/code.
