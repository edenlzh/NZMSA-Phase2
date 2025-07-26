# NZMSA-Phase2 project: Hand In Hand

## Overview
- [Introduction](#Introduction)
- [Theme Relevance](#Theme-Relevance)
- [Interesting Features](#Interesting-Features)
- [Checklist of Advanced Features](#Checklist-of-Advanced-Features)

## Introduction

This project is implemented according to the requirements of NZMSA 2025 Phase 2, and I have named it "Hand In Hand", a networking application for people to share skills and help each other. This application adopts frontend and backend separation deployment, with Azure Web App for the backend and Azure Static Web App for the frontend, and you can access it at [Hand In Hand](https://kind-dune-03fef7d00.2.azurestaticapps.net/). This URL is randomly assigned.

In this application, you can share your skills and seek help from the community. You can post skills by clicking the "Post Skill" button after logging in, and seek help by clicking the "Ask Help" button. You can also browse skills and help requests posted by other users and leave comments on any post. After logging in, you can click on your avatar in the navigation bar to access your profile page, where you can view and edit your personal information. If you have previously posted skills or help requests that are no longer relevant, you can delete them from the "My Skills" or "My Requests" pages. Please note that any deletion is irreversible, and once an account is deleted, it cannot be recovered. Please proceed with caution.

## Theme Relevance

The theme of this project is to create a platform that connects people especially students, allowing them to share their skills and assist one another. This aligns with the theme of networking and the NZMSA's goal of fostering community and collaboration among students.

People can post their skills and help requests, and others can comment on these posts to offer assistance or share their own experiences. This creates a supportive environment where users can learn from each other and build connections.

## Interesting Features

- **User Authentication**: Users can register, log in, and manage their profiles. Unregistered users can only view posts, while registered users can post skills and help requests, comment on posts.
- **Responsive Design**: The application is designed to be responsive, ensuring a good user experience on both desktop and mobile devices.
- **Dark Mode**: The application supports dark mode, allowing users to switch between light and dark themes for better readability and comfort.
- **Multi-language Support**: The application supports both Chinese and English languages, allowing users to choose their preferred language for the interface.
- **Comment Paging**: Comments on posts are paginated, allowing users to view comments in a structured manner without overwhelming them with too much information at once. One page has maximum 6 comments.
- **User Deletion**: Users can delete their accounts, which will also remove all their posts and comments from the application. This feature is irreversible, ensuring that users can manage their data effectively.

## Checklist of Advanced Features

- [x] **Unit testing components**: The application includes unit tests for backend components to ensure reliability and functionality.
- [x] **Redux state management**: The application uses Redux for state management, allowing for efficient data handling and updates across components.
- [x] **Support for light/dark mode switching**: The application supports light and dark mode switching, enhancing user experience and accessibility.
- [x] **End-to-end testing using Cypress**: The application includes end-to-end tests using Cypress to ensure that the entire user journey works as expected.
