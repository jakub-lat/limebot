<template>
  <v-container class="px-3">
    <v-alert type="info" text>
      Bot & website are under development
    </v-alert>
    <v-row align="center" no-gutters class="my-5 my-sm-12">
      <v-col cols="auto" class="px-4">
        <v-img src="@/assets/lime.png" width="150px"/>
      </v-col>
      <v-col class="px-4">
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
    
    <h1 class="display-1 mb-3" style="margin-top: 100px">Features</h1>
    <ul class="subtitle-1">
      <li>Very small functionality</li>
      <li>Beautiful panel with only one setting</li>
      <li>Fun commands</li>
      <li>More coming soon!</li>
    </ul>

    <a href="https://discord.gg/9w9EfWh" class="mt-10"><img src="https://discordapp.com/api/guilds/703280508640952401/embed.png?style=banner3"/></a>

  </v-container>
</template>

<script>
export default {
  name: 'Home'
};
</script>
