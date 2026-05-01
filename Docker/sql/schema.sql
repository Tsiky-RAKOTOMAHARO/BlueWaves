USE mysql_serveur;

CREATE TABLE FOURNISSEUR(
    RefFournisseur INT AUTO_INCREMENT PRIMARY KEY,
    NomFournisseur VARCHAR(150) NOT NULL,
    PrenomFournisseur VARCHAR(150),
    TelephoneFournisseur VARCHAR(25) NOT NULL
);

CREATE TABLE STOCK(
    NumeroStock INT AUTO_INCREMENT PRIMARY KEY,
    NomStock VARCHAR(25) NOT NULL
);

CREATE TABLE PRODUIT(
    CodeProduit INT AUTO_INCREMENT PRIMARY KEY,
    NomProduit VARCHAR(150),
    Prix INT,
    Statut BOOLEAN
);

CREATE TABLE APPROVISIONNEMENT(
    IdApp INT AUTO_INCREMENT PRIMARY KEY,
    RefFournisseur INT,
    CodeProduit INT,
    NumeroStock INT,
    Quantite INT NOT NULL,
    DateReception DATE NOT NULL,
    Certificat VARCHAR(150) NOT NULL,

    FOREIGN KEY (RefFournisseur) REFERENCES FOURNISSEUR(RefFournisseur),
    FOREIGN KEY (CodeProduit) REFERENCES PRODUIT(CodeProduit),
    FOREIGN KEY (NumeroStock) REFERENCES STOCK(NumeroStock)
);

CREATE TABLE STOCK_PRODUIT(
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NumeroStock INT NOT NULL,
    CodeProduit INT NOT NULL,
    Quantite INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_StockProduit_Stock
        FOREIGN KEY (NumeroStock) REFERENCES STOCK(NumeroStock),
    CONSTRAINT FK_StockProduit_Produit
        FOREIGN KEY (CodeProduit) REFERENCES PRODUIT(CodeProduit),
    CONSTRAINT UQ_StockProduit_Stock_Produit UNIQUE (NumeroStock, CodeProduit),
    CONSTRAINT CHK_StockProduit_Quantite CHECK (Quantite >= 0)
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
    RefClient INT NOT NULL, 
    NumeroExport INT NOT NULL,
    Destination VARCHAR(150) NOT NULL,
    DateCommande DATE NOT NULL,

    CONSTRAINT FK_Commande_Client 
        FOREIGN KEY (RefClient) REFERENCES CLIENT(RefClient),

    CONSTRAINT FK_Commande_Export 
        FOREIGN KEY (NumeroExport) REFERENCES EXPORT(NumeroExport)
);

CREATE TABLE ACHAT(
    IdAchat INT AUTO_INCREMENT PRIMARY KEY,
    CodeProduit INT NOT NULL, 
    NumeroCommande INT NOT NULL,
    NumeroStock INT NOT NULL,
    Quantite INT NOT NULL,

    CONSTRAINT FK_Achat_Produit 
        FOREIGN KEY (CodeProduit) REFERENCES PRODUIT(CodeProduit),

    CONSTRAINT FK_Achat_Commande 
        FOREIGN KEY (NumeroCommande) REFERENCES COMMANDE(NumeroCommande),

    CONSTRAINT FK_Achat_Stock                          -- ← à ajouter
        FOREIGN KEY (NumeroStock) REFERENCES STOCK(NumeroStock)
);
