// import { gql } from 'apollo-server-express';
// import { GraphQLUpload } from 'graphql-upload';

// // Define your types
// const typeDefs = gql`
//   scalar Upload

//   type Book {
//     title: String
//     author: String
//     isbn: String
//     publisher: String
//     availableCopies: Int
//     publishedYear: Int
//     genre: String
//   }

//   input BookInput {
//     title: String!
//     author: String!
//     isbn: String!
//     publisher: String!
//     availableCopies: Int!
//     publishedYear: Int!
//     genre: String!
//   }

//   type Mutation {
//     addBook(book: BookInput!, files: [Upload!]): Book
//   }
// `;

// // Define your resolvers
// const resolvers = {
//   Upload: GraphQLUpload, // Adding the Upload scalar
//   Mutation: {
//     addBook: async (parent: unknown, { book, files }: { book: unknown; files: unknown[] }) => {
//       // Handle the book creation and file upload logic here
//       console.log(book, files);
//       return book; // Return the created book
//     },
//   },
// };

// export { typeDefs, resolvers };
