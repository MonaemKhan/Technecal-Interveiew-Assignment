# Technecal-Interveiew-Assignment

A full-stack project developed as part of a technical interview assignment.  
It demonstrates a **modern web application** built with:

- **Angular** (Frontend)
- **.NET Web API** (Backend)
- **MS SQL Server** (Database)

The system showcases CRUD operations, database connectivity, and client-server communication.

---

## 🚀 Features

- **Appointment** – Add, update, delete, and search and sort appointment list in grid.
- **Prescription** – Make prescription for appointment, with in line edit also add new row and delete exiting row.  
- **Reports** – View report of each appointment.

---

## 🛠 Tech Stack

- **Frontend:** Angular 14+, TypeScript
- **Backend:** .NET 8.0 Web API  
- **Database:** Microsoft SQL Server  
- **ORM:** Entity Framework Core  
- **Tools:** Swagger/Postman (API testing), Git(Version control).



📂 Project Structure

Technecal-Interveiew-Assignment/\
├── Medical-Appoinment-System-API/ # Backend - .NET Web API\
│ ├── Controllers/ # API Controllers\
│ ├── Models/ # Entity classes\
│ ├── DBConnectionContext/ # DbContext & EF Migrations\
│ ├── Migrations/ # Migration File Database Migration\
│ ├── appsettings.json # DB connection settings # Email settings\
│ └── Program.cs\
│
├── Medical-Appointment-System/ # Frontend - Angular\
│ ├── src/\
│ │ ├── app/\
│ │ │ ├── core/ # Reusable UI components\
│ │ │ ├── services/ # API communication\
│ │ │ ├── models/ # TypeScript interfaces\
│ │ │ ├── pages/ # Feature pages (CRUD, reports)\
│ │ │ └── app.module.ts\
│ │ ├── index.html\
│ │ └── styles.css\
│ └── package.json\
│
├── .gitignore\
└── README.md


---

## ⚙️ Setup Instructions

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/MonaemKhan/Technecal-Interveiew-Assignment.git
cd Technecal-Interveiew-Assignment
```
- **open cmd**
```
- cd Medical-Appoinment-System-API
```
- set connection string & email setting in **appsettings.json**
```
dotnet ef database update
dotnet run
```

- Go back to **Technecal-Interveiew-Assignment**
```
cd ..
cd Medical-Appointment-System
npm install
ng serve -o
```
Frontend runs at: http://localhost:4200/

## 📖 API Endpoints

### Appoinment
##### Endpoint
```
api/Apointment	(GET)	Get all apointment)
api/Apointment/{id}	(GET)	Get appointment by ID)
api/Apointment	(POST)	(Add a new appointment)
api/Apointment/{id}	(PUT)	(Update a appointment)
api/Apointment/{id}	(DELETE)	(Delete a medicine)
```
### Doctor
##### Endpoint
```
api/Doctor	(GET)	Get all Doctor)
api/Doctor/{id}	(GET)	Get Doctor by ID)
```
### Patient
##### Endpoint
```
api/Patient	(GET)	Get all Doctor)
api/Patient/{id}	(GET)	Get Doctor by ID)
```

## 📸 Screenshots

### New Apointment
![1](https://github.com/MonaemKhan/Technecal-Interveiew-Assignment/blob/main/Screenshots/1.png)

### Apointment Save
![3](https://github.com/MonaemKhan/Technecal-Interveiew-Assignment/blob/main/Screenshots/2.png)

### View All Apointment Liist
![4](https://github.com/MonaemKhan/Technecal-Interveiew-Assignment/blob/main/Screenshots/4.png)

### Search & sort Apointment Liist
![5](https://github.com/MonaemKhan/Technecal-Interveiew-Assignment/blob/main/Screenshots/5.png)

### Prescription
![16](https://github.com/MonaemKhan/Technecal-Interveiew-Assignment/blob/main/Screenshots/16.png)

### Pdf Report
![20](https://github.com/MonaemKhan/Technecal-Interveiew-Assignment/blob/main/Screenshots/20.png)


# 📌 Author

👤 M.A. Monaem Khan\
🔗 [GitHub Profile](https://github.com/MonaemKhan)
