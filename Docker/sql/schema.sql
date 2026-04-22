USE mysql_serveur;

CREATE TABLE FOURNISSEUR(
    RefFournisseur INT AUTO_INCREMENT PRIMARY KEY,
    NomFournisseur VARCHAR(150) NOT NULL,
    PrenomFournisseur VARCHAR(150),
    TelephoneFournisseur VARCHAR(25) NOT NULL
);

CREATE TABLE STOCK(
    NumeroStock INT AUTO_INCREMENT PRIMARY KEY,
    Type VARCHAR(25) NOT NULL
);

CREATE TABLE PRODUIT(
    CodeProduit INT AUTO_INCREMENT PRIMARY KEY,
    NumeroStock INT,
    NomProduit VARCHAR(150),
    QuantiteProduit INT NOT NULL,
    Date_reception DATE NOT NULL,
    Statut BOOLEAN,

    FOREIGN KEY (NumeroStock) REFERENCES STOCK(NumeroStock)
);

CREATE TABLE APPROVISIONNEMENT(
    IdApp INT AUTO_INCREMENT PRIMARY KEY,
    RefFournisseur INT,
    CodeProduit INT, 
    Certificat VARCHAR(150) NOT NULL,

    FOREIGN KEY (RefFournisseur) REFERENCES FOURNISSEUR(RefFournisseur),
    FOREIGN KEY (CodeProduit) REFERENCES PRODUIT(CodeProduit)
);

CREATE TABLE CLIENT(
    RefClient INT AUTO_INCREMENT PRIMARY KEY,
    NomClient VARCHAR(150) NOT NULL,
    PrenomClient VARCHAR(150),
    Telephone VARCHAR(25) NOT NULL
);

CREATE TABLE EXPORT(
    NumeroExport INT AUTO_INCREMENT PRIMARY KEY,
    Delai INT NOT NULL
);

CREATE TABLE COMMANDE(
    NumeroCommande INT AUTO_INCREMENT PRIMARY KEY,
    RefClient INT, 
    NumeroExport INT,
    Destination VARCHAR(150) NOT NULL,
    Date_commande DATE NOT NULL,

    FOREIGN KEY (RefClient) REFERENCES CLIENT(RefClient),
    FOREIGN KEY (NumeroExport) REFERENCES EXPORT(NumeroExport)
);

CREATE TABLE ACHAT(
    IdAchat INT AUTO_INCREMENT PRIMARY KEY,
    CodeProduit INT, 
    NumeroCommande INT,
    Quantite INT NOT NULL,

    FOREIGN KEY (CodeProduit) REFERENCES PRODUIT(CodeProduit),
    FOREIGN KEY (NumeroCommande) REFERENCES COMMANDE(NumeroCommande)
);