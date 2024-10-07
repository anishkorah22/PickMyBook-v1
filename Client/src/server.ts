// import express from 'express';
// import { ApolloServer } from 'apollo-server-express';
// import { graphqlUploadExpress } from 'graphql-upload';
// import { typeDefs, resolvers } from './schema'; // Ensure this is also in TypeScript

// const app = express();

// // Middleware for handling file uploads
// app.use(graphqlUploadExpress());

// const server = new ApolloServer({
//   typeDefs,
//   resolvers,
// });

// server.applyMiddleware({ app });

// const PORT = process.env.PORT || 4000;

// app.listen(PORT, () => {
//   console.log(`🚀 Server ready at http://localhost:${PORT}${server.graphqlPath}`);
// });
