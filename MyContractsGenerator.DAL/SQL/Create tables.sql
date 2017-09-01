
set xact_abort on;
begin transaction;

CREATE TABLE collaborateur
(
    id_collaborateur    INT identity(1,1)   NOT NULL,
    nom                 NVARCHAR(50)        NOT NULL,
    prenom              NVARCHAR(50)        NOT NULL,
    constraint pk_collaborateur PRIMARY KEY (id_collaborateur)
);

CREATE TABLE departement
(
    id_departement          int identity(1,1)   NOT NULL,
    id_departement_parent   int                     null,
    id_responsable          INT                     NULL,
    libelle                 NVARCHAR(100)       NOT NULL,
    constraint pk_departement PRIMARY KEY (id_departement),
    constraint fk_departement_parent FOREIGN KEY (id_departement_parent) REFERENCES departement (id_departement),
    constraint fk_departement_responsable FOREIGN KEY (id_responsable) REFERENCES collaborateur (id_collaborateur)
);

CREATE TABLE collaborateur_departement
(
   id_collaborateur     int NOT NULL,
   id_departement       int NOT NULL,
   constraint pk_collaborateur_departement PRIMARY KEY (id_collaborateur, id_departement),
   constraint fk_collaborateur_departement_collaborateur FOREIGN KEY (id_collaborateur)  REFERENCES collaborateur (id_collaborateur),
   constraint fk_collaborateur_departement_departement FOREIGN KEY (id_departement) REFERENCES departement (id_departement)
);

--rollback;
commit;
go

set xact_abort on;
begin transaction;

SET IDENTITY_INSERT collaborateur ON;

insert into collaborateur (id_collaborateur, nom, prenom) values
    (1, 'D''anglette', 'La Reine'),
    (2, 'Moore', 'Roger'),
    (3, 'Kiou', 'Petit'),
    (4, 'Bonisseur de la batte', 'Hubert'),
    (5, 'Connerie', 'Chaune'),
    (6, 'Secrétaire', 'La');

SET IDENTITY_INSERT collaborateur OFF;

SET IDENTITY_INSERT departement ON;

insert into departement (id_departement, id_departement_parent, libelle, id_responsable) values
    (1, null, 'UK', 1),
    (2, 1, 'MI6', 2),
    (3, 2, 'Section bricolage', 3),
    (4, 2, 'Espionnage', 4);

SET IDENTITY_INSERT departement OFF;

insert into collaborateur_departement (id_departement, id_collaborateur) values
    (2, 6),
    (3, 4),
    (4, 4),
    (4, 5);

--rollback;
commit;
