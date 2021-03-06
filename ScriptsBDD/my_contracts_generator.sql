-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Client :  127.0.0.1
-- Généré le :  Jeu 07 Septembre 2017 à 18:37
-- Version du serveur :  5.6.17
-- Version de PHP :  5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de données :  `my_contracts_generator`
--
CREATE DATABASE IF NOT EXISTS `my_contracts_generator` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `my_contracts_generator`;

-- --------------------------------------------------------

--
-- Structure de la table `administrator`
--

DROP TABLE IF EXISTS `administrator`;
CREATE TABLE IF NOT EXISTS `administrator` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `password` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `email` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `active` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci AUTO_INCREMENT=5 ;

--
-- Contenu de la table `administrator`
--

INSERT INTO `administrator` (`id`, `login`, `password`, `email`, `active`) VALUES
(1, 'c.fevre', '89D920829A794322E6F27178552359D1503643DF50DA827ECCB766C3A6025E95', 'c.fevre166@gmail.com', 1),
(2, 'j.collier', '89D920829A794322E6F27178552359D1503643DF50DA827ECCB766C3A6025E95', 'fux.prod@gmail.com', 1),
(3, 'maylis.dlb', '89D920829A794322E6F27178552359D1503643DF50DA827ECCB766C3A6025E95', 'maylis.dlb@hotmail.fr', 1),
(4, 'b.ernst', '89D920829A794322E6F27178552359D1503643DF50DA827ECCB766C3A6025E95', 'ernst.baptistin@gmail.com', 1);

-- --------------------------------------------------------

--
-- Structure de la table `answer`
--

DROP TABLE IF EXISTS `answer`;
CREATE TABLE IF NOT EXISTS `answer` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `form_answer_id` int(11) NOT NULL,
  `question_id` int(11) NOT NULL,
  `answer_value` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `answer_id_2` (`id`),
  KEY `answer_id` (`id`),
  KEY `form_answer_id` (`form_answer_id`),
  KEY `question_id` (`question_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci AUTO_INCREMENT=89 ;

-- --------------------------------------------------------

--
-- Structure de la table `collaborator`
--

DROP TABLE IF EXISTS `collaborator`;
CREATE TABLE IF NOT EXISTS `collaborator` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `firstname` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `lastname` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `email` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `active` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci AUTO_INCREMENT=8 ;

--
-- Contenu de la table `collaborator`
--

INSERT INTO `collaborator` (`id`, `firstname`, `lastname`, `email`, `active`) VALUES
(1, 'Julien', 'Collier', 'fux.prod@gmail.com', 1),
(5, 'test', 'test', 'test@test.test', 0),
(6, 'Clément', 'Fevre', 'c.fevre166@gmail.com', 1),
(7, 'Franco', 'Robert', 'testMCG@yopmail.com', 1);

-- --------------------------------------------------------

--
-- Structure de la table `collaborator_has_role`
--

DROP TABLE IF EXISTS `collaborator_has_role`;
CREATE TABLE IF NOT EXISTS `collaborator_has_role` (
  `id_collaborator` int(11) NOT NULL,
  `id_role` int(11) NOT NULL,
  PRIMARY KEY (`id_collaborator`,`id_role`),
  KEY `id_role` (`id_role`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci;

--
-- Contenu de la table `collaborator_has_role`
--

INSERT INTO `collaborator_has_role` (`id_collaborator`, `id_role`) VALUES
(6, 7),
(6, 8),
(7, 8),
(1, 9),
(6, 9),
(1, 10),
(6, 10),
(7, 11);

-- --------------------------------------------------------

--
-- Structure de la table `form`
--

DROP TABLE IF EXISTS `form`;
CREATE TABLE IF NOT EXISTS `form` (
  `id` int(11) NOT NULL,
  `label` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `label` (`label`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci;

--
-- Contenu de la table `form`
--

INSERT INTO `form` (`id`, `label`) VALUES
(1, 'Formulaire MyContractsGenerator');

-- --------------------------------------------------------

--
-- Structure de la table `form_answer`
--

DROP TABLE IF EXISTS `form_answer`;
CREATE TABLE IF NOT EXISTS `form_answer` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `form_id` int(11) NOT NULL,
  `collaborator_id` int(11) NOT NULL,
  `role_id` int(11) NOT NULL,
  `replied` tinyint(1) NOT NULL,
  `last_collaborator_mail_time` datetime NOT NULL,
  `last_update` datetime NOT NULL,
  `password` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `admin_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `form_answer_id` (`id`),
  KEY `form_answer_id_2` (`id`),
  KEY `form_id` (`form_id`),
  KEY `collaborator_id` (`collaborator_id`),
  KEY `admin_id` (`admin_id`),
  KEY `role_id` (`role_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci AUTO_INCREMENT=20 ;

-- --------------------------------------------------------

--
-- Structure de la table `form_has_question`
--

DROP TABLE IF EXISTS `form_has_question`;
CREATE TABLE IF NOT EXISTS `form_has_question` (
  `id_form` int(11) NOT NULL,
  `id_question` int(11) NOT NULL,
  PRIMARY KEY (`id_form`,`id_question`),
  KEY `id_question` (`id_question`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci;

--
-- Contenu de la table `form_has_question`
--

INSERT INTO `form_has_question` (`id_form`, `id_question`) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 6),
(1, 7),
(1, 8),
(1, 9);

-- --------------------------------------------------------

--
-- Structure de la table `question`
--

DROP TABLE IF EXISTS `question`;
CREATE TABLE IF NOT EXISTS `question` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type_id` int(11) NOT NULL,
  `label` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `order` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `type_id` (`type_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci AUTO_INCREMENT=10 ;

--
-- Contenu de la table `question`
--

INSERT INTO `question` (`id`, `type_id`, `label`, `order`) VALUES
(1, 1, 'Nom', 1),
(2, 1, 'Prénom', 2),
(3, 1, 'Pseudo', 3),
(4, 4, 'Date de naissance', 5),
(6, 1, 'Ville de naissance', 6),
(7, 1, 'Adresse (domicile)', 7),
(8, 1, 'Adresse mail', 4),
(9, 1, 'Numéro de téléphone', 9);

-- --------------------------------------------------------

--
-- Structure de la table `question_type`
--

DROP TABLE IF EXISTS `question_type`;
CREATE TABLE IF NOT EXISTS `question_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `label` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci AUTO_INCREMENT=5 ;

--
-- Contenu de la table `question_type`
--

INSERT INTO `question_type` (`id`, `label`) VALUES
(1, 'text'),
(2, 'numeric'),
(3, 'boolean'),
(4, 'date');

-- --------------------------------------------------------

--
-- Structure de la table `role`
--

DROP TABLE IF EXISTS `role`;
CREATE TABLE IF NOT EXISTS `role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `label` varchar(255) COLLATE utf8_general_mysql500_ci NOT NULL,
  `active` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `label` (`label`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci AUTO_INCREMENT=12 ;

--
-- Contenu de la table `role`
--

INSERT INTO `role` (`id`, `label`, `active`) VALUES
(7, 'Développeur', 1),
(8, 'Guitariste', 1),
(9, 'Producteur', 1),
(10, 'Ingénieur du son', 1),
(11, 'Manutention', 1);

-- --------------------------------------------------------

--
-- Structure de la table `role_has_form`
--

DROP TABLE IF EXISTS `role_has_form`;
CREATE TABLE IF NOT EXISTS `role_has_form` (
  `id_role` int(11) NOT NULL,
  `id_form` int(11) NOT NULL,
  PRIMARY KEY (`id_role`,`id_form`),
  KEY `id_form` (`id_form`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci;

--
-- Contraintes pour les tables exportées
--

--
-- Contraintes pour la table `answer`
--
ALTER TABLE `answer`
  ADD CONSTRAINT `answer_ibfk_1` FOREIGN KEY (`form_answer_id`) REFERENCES `form_answer` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `answer_ibfk_2` FOREIGN KEY (`question_id`) REFERENCES `question` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Contraintes pour la table `collaborator_has_role`
--
ALTER TABLE `collaborator_has_role`
  ADD CONSTRAINT `collaborator_has_role_ibfk_1` FOREIGN KEY (`id_collaborator`) REFERENCES `collaborator` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `collaborator_has_role_ibfk_2` FOREIGN KEY (`id_role`) REFERENCES `role` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Contraintes pour la table `form_answer`
--
ALTER TABLE `form_answer`
  ADD CONSTRAINT `form_answer_ibfk_4` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `form_answer_ibfk_1` FOREIGN KEY (`form_id`) REFERENCES `form` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `form_answer_ibfk_2` FOREIGN KEY (`collaborator_id`) REFERENCES `collaborator` (`id`),
  ADD CONSTRAINT `form_answer_ibfk_3` FOREIGN KEY (`admin_id`) REFERENCES `administrator` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Contraintes pour la table `form_has_question`
--
ALTER TABLE `form_has_question`
  ADD CONSTRAINT `form_has_question_ibfk_1` FOREIGN KEY (`id_form`) REFERENCES `form` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `form_has_question_ibfk_2` FOREIGN KEY (`id_question`) REFERENCES `question` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Contraintes pour la table `question`
--
ALTER TABLE `question`
  ADD CONSTRAINT `question_ibfk_1` FOREIGN KEY (`type_id`) REFERENCES `question_type` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Contraintes pour la table `role_has_form`
--
ALTER TABLE `role_has_form`
  ADD CONSTRAINT `role_has_form_ibfk_1` FOREIGN KEY (`id_role`) REFERENCES `role` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `role_has_form_ibfk_2` FOREIGN KEY (`id_form`) REFERENCES `form` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
