# Library Management System

## Overview
The Library Management System is a comprehensive software solution designed to manage a library's operations. It includes modules for managing users, books, and various interactions such as borrowing, returning, and reserving books. This system is built using pure C# and is structured to be maintainable, scalable, and easy to understand.

## Modules

### 1. User Authentication
The User Authentication module handles all aspects of user management, including registration, login, logout, and account deletion. It ensures that only authenticated users can access the library's resources.

**Key Functionalities:**
- **Registration:** Allows new users to register by providing a username, password, and email.
- **Login:** Authenticates users and grants them access to the system.
- **Logout:** Logs out currently logged-in users.
- **Add/Delete Books:** Restricts these operations to logged-in users only.
- **Account Deletion:** Allows users to delete their accounts.

**Example Usage:**
- A user can register, log in, perform operations like adding books (if logged in), and then log out or delete their account.

For more details, please refer to the [User Authentication Design Document](./DesignDoc/UserAuthentication.md).

### 2. Book Management
The Book Management module is responsible for maintaining the library's book catalog. It covers operations like adding new books, deleting books, searching for books by title or author, and displaying all books. Additionally, it manages the borrowing, returning, and reserving of books, while keeping all metadata up-to-date.

**Key Functionalities:**
- **Adding Books:** Allows authorized users to add new books to the catalog.
- **Updating Books:** Allows authorized users to update book metadata.
- **Deleting Books:** Allows authorized users to delete books from the catalog.
- **Borrowing/Returning Books:** Manages the borrowing and returning of books, including checking for overdue status.
- **Reserving Books:** Allows users to reserve books that are not currently borrowed.
- **Displaying Books:** Allows users to view all books or search for books by title or author.

For more details, please refer to the [Book Management Design Document](./DesignDoc/BookManagement.md).

## How to Run the Application

Follow these steps to run the Library Management System on your local machine:

### 1. Clone the Repository

First, clone the repository to your local machine using the following command:

```bash
git clone https://github.com/LemmarWilson/Library-Management-System.git
```

Navigate to the project directory:

```bash
cd Library-Management-System
```

### 2. Build the Project

Once inside the project directory, build the project using the .NET CLI:

```bash
dotnet build
```

This will compile the project and prepare it for execution.

### 3. Run the Application

After successfully building the project, you can run the application using the following command:

```bash
dotnet run
```

This will start the application, and you can begin interacting with the Library Management System via the command-line interface.

### Contributions
This project is the final submission for CSC200 - Intermediate Programming. The development team consists of <span style="color:#F0EAD6;">**Lemmar Wilson**</span>, <span style="color:#F0EAD6;">**Amber Mattoni**</span>, and <span style="color:#F0EAD6;">**John P. McDurmon**</span>
. We have collaborated to create a comprehensive Library Management System, demonstrating our skills in C# programming, software design, and project management.