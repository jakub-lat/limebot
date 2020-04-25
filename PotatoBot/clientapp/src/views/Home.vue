<template>
  <v-container class="px-3">
    <v-alert type="info" text>
      Bot & website are under development
    </v-alert>
    <v-row align="center" no-gutters class="my-5 my-sm-12">
      <v-col cols="auto" class="px-4">
        <v-img src="@/assets/lime2.png" width="150px"/>
      </v-col>
      <v-col class="px-4" cols="12" sm="">
        <h1 class="display-3 mb-3 mt-5 font-weight-bold" id="start">
          Lime
        </h1>
        <h2 class="display-1">
          Keep calm. Lime is just there.
        </h2>
        <v-btn color="primary" class="mt-4 mr-3" disabled>
          Invite bot
        </v-btn>
        <v-btn color="secondary" class="mt-4" to="/manage">
          Dashboard
        </v-btn>
      </v-col>
    </v-row>
    
    <h1 class="display-1 mb-3" style="margin-top: 100px" id="features">Features</h1>
    <ul class="subtitle-1">
      <li>Moderation commands</li>
      <li>Memes, dadjokes and posts from reddit</li>
      <li>Autoroles & welcome/leave messages</li>
      <li>Intuitive dashboard</li>
      <li>More coming soon!</li>
    </ul>

    <a href="https://discord.gg/9w9EfWh"><img class="mt-10" src="https://discordapp.com/api/guilds/703280508640952401/embed.png?style=banner3"/></a>

  </v-container>
</template>

<script>
export default {
  name: 'Home'
};
</script>
