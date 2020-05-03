<template>
  <v-container class="px-2 px-md-12">
    <v-row align="center" no-gutters class="my-5 my-sm-12">
      <v-col cols="auto" class="px-4">
        <v-img src="@/assets/lime2.png" width="150px"/>
      </v-col>
      <v-col class="px-4" cols="12" sm="">
        <h1 class="display-3 mb-3 mt-5 font-weight-bold" id="start">
          Lime
        </h1>
        <h2 class="headline">
          Boost your discord server with Lime power!
        </h2>
        <v-btn color="primary darken-1" class="mt-4 mr-5" to="/invite" large>
           <v-icon left>mdi-account-plus</v-icon>
          Invite the bot!
        </v-btn>
        <v-btn color="secondary lighten-1" class="mt-4" to="/manage" large>
          <v-icon left>mdi-login-variant</v-icon>
          Login to Dashboard
        </v-btn>
      </v-col>
    </v-row>
    
    <h1 class="display-1 mb-4 mx-5" style="margin-top: 100px">Features</h1>

    <div class="d-flex flex-wrap">
      <v-hover
        v-slot:default="{ hover }"
        v-for="(i, n) in features" :key="n"
      >
        <v-card
          class="mx-auto mx-sm-7 my-4" max-width="260" 
          color="grey darken-4"
          :elevation="hover ? 12 : 3"
          link :to="i.to || '#'"
        >
          <div class="text-center mt-4 ">
            <v-icon class="display-3" :color="i.color || 'primary'">{{i.icon}}</v-icon>
          </div>
          <v-card-title class="mb-3">
            <h2 class="headline font-weight-medium">{{i.title}}</h2>
          </v-card-title>
          <v-card-subtitle class="subtitle-1">
            {{i.subtitle}}
          </v-card-subtitle>
        </v-card>
      </v-hover>
    </div>

    <v-row style="margin-top: 50px; margin-bottom: 100px;">
      <v-col md="6" xl="5">
        <h1 class="display-1 mb-4 mx-5">Commands</h1>
        <v-hover v-slot:default="{ hover }">
          <v-card 
            color="grey darken-4" :elevation="hover ? 6 : 3" class="mx-6 my-4" max-width="400"
            link to="/commands"
          >
            <v-list-item>
              <v-list-item-icon>
                <v-icon color="primary">mdi-view-list</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>Explore all commands</v-list-item-title>
                <v-list-item-subtitle>See what you can use!</v-list-item-subtitle>
              </v-list-item-content>

              <v-list-item-action>
                    <v-icon>mdi-arrow-right</v-icon>
                  </v-list-item-action>
            </v-list-item>
          </v-card>
        </v-hover>
      </v-col>
      <v-col md="6" xl="5">
        <h1 class="display-1 mb-4 mx-5" >Discord server</h1>
        <v-hover v-slot:default="{ hover }">
          <v-card 
            color="grey darken-4" :elevation="hover ? 6 : 3" class="mx-6 my-4" max-width="400" 
            link href="https://discord.gg/9w9EfWh"
          >
            <v-list-item>
              <v-list-item-icon>
                <v-icon color="indigo accent-2">mdi-discord</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title>Join Lime Bot Discord server</v-list-item-title>
                <v-list-item-subtitle>Come chat with us!</v-list-item-subtitle>
              </v-list-item-content>

              <v-list-item-action>
                    <v-icon>mdi-arrow-right</v-icon>
                  </v-list-item-action>
            </v-list-item>
          </v-card>
        </v-hover>
      </v-col>
    </v-row>

    <v-footer absolute class="px-12 py-3">
      <v-col
        cols="12"
      >
        &copy; {{ new Date().getFullYear() }} the <strong>Lime Bot Team</strong>
      </v-col>
    </v-footer>
  </v-container>
</template>

<script>
export default {
  name: 'Home',
  data: ()=>({
    features: [
      {
        icon: 'mdi-hammer',
        title: 'Moderate like a pro.',
        subtitle: 'Lime Bot offers variety of commands that can help you moderate your server with ease.',
        color: 'lime'
      },
      {
        icon: 'mdi-view-dashboard',
        title: 'Intuitive dashboard.',
        subtitle: 'Allows you to manage the settings very easily and customize the bot to its limits.',
        to: '/manage'
      },
      {
        icon: 'mdi-music',
        title: 'Enjoy music you like.',
        subtitle: 'Play your favorite songs, create server playlists and listen with your friends!',
        color: 'red accent-2'
      },
      {
        icon: 'mdi-account-plus',
        title: 'Say hi to new users.',
        subtitle: 'Customize join / leave actions, like autoroles and welcome & leave messages.',
        color: 'cyan accent-4'
      }
      // stats, reactionroles
    ]
  })
};
</script>
