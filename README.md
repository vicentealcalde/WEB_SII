
# WEB_SII

# _*Base de datos*_
se debe poner en el sql server 

    CREATE DATABASE Escrituras;

    GO
    USE Escrituras;
    GO
    CREATE TABLE Escritura (
    NumAtencion INT IDENTITY(1,1) PRIMARY KEY,
    CNE VARCHAR(50) NOT NULL,
    Comuna VARCHAR(50) NOT NULL,
    Manzana VARCHAR(50) NOT NULL,
    Predio VARCHAR(50) NOT NULL,
    Fojas INT NOT NULL,
    FechaInscripcion DATE NOT NULL,
    NumeroInscripcion VARCHAR(50) NOT NULL
    );
    GO
    CREATE TABLE Enajenante (
    IdEnajenante INT IDENTITY(1,1) PRIMARY KEY,
    NumAtencion INT NOT NULL,
    RUN_RUT VARCHAR(50) NOT NULL,
    PorcentajeDerecho FLOAT NOT NULL,
    PorcentajeDerechoNoAcreditado BIT NOT NULL,
    FOREIGN KEY (NumAtencion) REFERENCES Escritura(NumAtencion)
    );
    GO
    CREATE TABLE Adquirente (
    IdAdquirente INT IDENTITY(1,1) PRIMARY KEY,
    NumAtencion INT NOT NULL,
    RUN_RUT VARCHAR(50) NOT NULL,
    PorcentajeDerecho FLOAT NOT NULL,
    PorcentajeDerechoNoAcreditado BIT NOT NULL,
    FOREIGN KEY (NumAtencion) REFERENCES Escritura(NumAtencion)
    );
    GO
    USE Escrituras;
    GO

    SET IDENTITY_INSERT Escritura ON;

    INSERT INTO Escritura (NumAtencion, CNE, Comuna, Manzana, Predio, Fojas, FechaInscripcion, NumeroInscripcion) 
    VALUES (1, 'CNE-001', 'Comuna A', 'Manzana 1', 'Predio 1', 1, '2022-01-01', 'NI-001'),
        (2, 'CNE-002', 'Comuna B', 'Manzana 2', 'Predio 2', 2, '2022-02-02', 'NI-002'),
        (3, 'CNE-003', 'Comuna C', 'Manzana 3', 'Predio 3', 3, '2022-03-03', 'NI-003');

    SET IDENTITY_INSERT Escritura OFF;

    INSERT INTO Enajenante (NumAtencion, RUN_RUT, PorcentajeDerecho, PorcentajeDerechoNoAcreditado)
    VALUES (1, '11111111-1', 50.0, 0),
        (2, '22222222-2', 75.0, 1),
        (2, '33333333-3', 25.0, 0),
        (3, '44444444-4', 100.0, 0);

    INSERT INTO Adquirente (NumAtencion, RUN_RUT, PorcentajeDerecho, PorcentajeDerechoNoAcreditado)
    VALUES (1, '55555555-5', 100.0, 0),
        (2, '66666666-6', 100.0, 0),
        (3, '77777777-7', 50.0, 0),
        (3, '88888888-8', 50.0, 0);
luego se le debe añadir lo la siguiente base de datos :

    CREATE DATABASE Escrituras;

    GO
    USE Escrituras;
    GO
    CREATE TABLE Escritura (
    NumAtencion INT IDENTITY(1,1) PRIMARY KEY,
    CNE VARCHAR(50) NOT NULL,
    Comuna VARCHAR(50) NOT NULL,
    Manzana VARCHAR(50) NOT NULL,
    Predio VARCHAR(50) NOT NULL,
    Fojas INT NOT NULL,
    FechaInscripcion DATE NOT NULL,
    NumeroInscripcion VARCHAR(50) NOT NULL
    );
    GO
    CREATE TABLE Enajenante (
    IdEnajenante INT IDENTITY(1,1) PRIMARY KEY,
    NumAtencion INT NOT NULL,
    RUN_RUT VARCHAR(50) NOT NULL,
    PorcentajeDerecho FLOAT NOT NULL,
    PorcentajeDerechoNoAcreditado BIT NOT NULL,
    FOREIGN KEY (NumAtencion) REFERENCES Escritura(NumAtencion)
    );
    GO
    CREATE TABLE Adquirente (
    IdAdquirente INT IDENTITY(1,1) PRIMARY KEY,
    NumAtencion INT NOT NULL,
    RUN_RUT VARCHAR(50) NOT NULL,
    PorcentajeDerecho FLOAT NOT NULL,
    PorcentajeDerechoNoAcreditado BIT NOT NULL,
    FOREIGN KEY (NumAtencion) REFERENCES Escritura(NumAtencion)
    );
    GO
    USE Escrituras
    GO

    CREATE TABLE MULTIPROPIETARIO (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Comuna VARCHAR(50) NOT NULL,
        Manzana INT NOT NULL,
        Predio INT NOT NULL,
        RUN_RUT VARCHAR(20) NOT NULL,
        PorcentajeDerecho FLOAT NOT NULL,
        Fojas INT NOT NULL,
        AnoInscripcion INT NOT NULL,
        NumeroInscripcion INT NOT NULL,
        FechaInscripcion DATE NOT NULL,
        AnoVigenciaInicial INT NOT NULL,
        AnoVigenciaFinal INT 
    )
    GO

    INSERT INTO MULTIPROPIETARIO (Comuna, Manzana, Predio, RUN_RUT, PorcentajeDerecho, Fojas, AnoInscripcion, NumeroInscripcion, FechaInscripcion, AnoVigenciaInicial, AnoVigenciaFinal)
    VALUES ('Santiago', 1, 1, '11111111-1', 50.0, 1, 2022, 1234, '2022-01-01', 2022, 2030),
        ('Santiago', 1, 1, '22222222-2', 50.0, 1, 2022, 1234, '2022-01-01', 2022, 2030)
    GO

    INSERT INTO MULTIPROPIETARIO (Comuna, Manzana, Predio, RUN_RUT, PorcentajeDerecho, Fojas, AnoInscripcion, NumeroInscripcion, FechaInscripcion, AnoVigenciaInicial, AnoVigenciaFinal)
    VALUES ('Concepción', 2, 3, '33333333-3', 25.0, 2, 2023, 5678, '2023-01-01', 2023, 2033),
        ('Concepción', 2, 3, '44444444-4', 50.0, 2, 2023, 5678, '2023-01-01', 2023, 2033),
        ('Concepción', 2, 3, '55555555-5', 25.0, 2, 2023, 5678, '2023-01-01', 2023, 2033)
    GO
# Cosas que se deben cambiar (dentro del codigo) para que funcione
En appsettings.json, linea 10 y en EscriturasContext.cs en la línea 26

se debe cambiar el
    
    server=.\\SQLEXPRESS

por

    server={nombre_del_servidor_en_sql_server}\\SQLEXPRESS

# Flujo

en la pagina de inicio hay que ir a la pestaña de escritura (en la barra de arriba de la pagina) donde estara un index, en forma de grilla con los datos de la escritura, tambien esta la opcion de crear una escritura, de editar, ver y eliminar la escritura

tambien arriba en las pestañas al lado derecho esta el boton de multipropietarios donde solo se mostrara un _*buscador o filtrador*_ para la tabla que esta abajo. tambien se debera cargar manualmente los datos de los multipropietario, como dice el enunciado (eso si puede que difiera un poco el modelo que nosostros tenemos con el que tiene, asi que revise el modelo antes )

    CREATE TABLE MULTIPROPIETARIO (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Comuna VARCHAR(50) NOT NULL,
        Manzana INT NOT NULL,
        Predio INT NOT NULL,
        RUN_RUT VARCHAR(20) NOT NULL,
        PorcentajeDerecho FLOAT NOT NULL,
        Fojas INT NOT NULL,
        AnoInscripcion INT NOT NULL,
        NumeroInscripcion INT NOT NULL,
        FechaInscripcion DATE NOT NULL,
        AnoVigenciaInicial INT NOT NULL,
        AnoVigenciaFinal INT 
    )
tambien es necesario agregar la siguiente consulta sql al sql server despues de poner los datos, SIEMPRE SE DEBE PONER AL FINAL

    UPDATE MULTIPROPIETARIO
    SET [AnoVigenciaFinal] = 0
    WHERE [AnoVigenciaFinal] IS NULL;

# EJEMPLO
    GO
    USE Escrituras;
    
    GO
    INSERT INTO MULTIPROPIETARIO (Comuna, Manzana, Predio, RUN_RUT, PorcentajeDerecho, Fojas, AnoInscripcion, NumeroInscripcion, FechaInscripcion, AnoVigenciaInicial, AnoVigenciaFinal)
    VALUES ('Comuna A', 2, 3, '33333333-3', 25.0, 2, 2023, 5678, '2023-01-01', 2023, NULL),
        ('Comuna A', 2, 3, '44444444-4', 50.0, 2, 2022, 5678, '2023-01-01', 2023, 2023),
        ('Comuna A', 2, 3, '55555555-5', 25.0, 2, 2023, 5678, '2023-01-01', 2023, Null)
    GO
    
    UPDATE MULTIPROPIETARIO
    SET [AnoVigenciaFinal] = 0
    WHERE [AnoVigenciaFinal] IS NULL;