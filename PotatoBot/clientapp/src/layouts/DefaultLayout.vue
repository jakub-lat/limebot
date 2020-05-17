<template>
<v-app>
    <v-navigation-drawer app temporary v-model="drawer" class="d-sm-none">
        <v-list-item>
            <v-list-item-avatar>
                <v-img src="@/assets/lime2.png"/>
            </v-list-item-avatar>
            <v-list-item-content>
            <v-list-item-title class="title">
                Lime
            </v-list-item-title>
            <v-list-item-subtitle>
                A multi-purpose Discord Bot
            </v-list-item-subtitle>
            </v-list-item-content>
        </v-list-item>
        <v-divider></v-divider>
        <v-list dense>
            <v-list-item link to="/">
            <v-list-item-action>
                <v-icon>mdi-home</v-icon>
            </v-list-item-action>
            <v-list-item-content>
                <v-list-item-title>Start</v-list-item-title>
            </v-list-item-content>
            </v-list-item>
            <v-list-item link to="/invite">
            <v-list-item-action>
                <v-icon>mdi-account-plus</v-icon>
            </v-list-item-action>
            <v-list-item-content>
                <v-list-item-title>Invite bot</v-list-item-title>
            </v-list-item-content>
            </v-list-item>

            <v-list-item link to="/commands">
            <v-list-item-action>
                <v-icon>mdi-view-list</v-icon>
            </v-list-item-action>
            <v-list-item-content>
                <v-list-item-title>Commands</v-list-item-title>
            </v-list-item-content>
            </v-list-item>
        </v-list>
    </v-navigation-drawer>
    <v-app-bar app clipped-left color="primary">
        <v-app-bar-nav-icon @click.stop="drawer = !drawer" class="d-sm-none" />
        <!--v-toolbar-title class="ml-4 mr-4">POTATO</v-toolbar-title-->
        <div class="ml-5 d-none d-sm-block">
            <v-btn text to="/">
                <v-icon left>mdi-home</v-icon>
                <span>Start</span>
            </v-btn>
            <v-btn text to="/invite">
                <v-icon left>mdi-account-plus</v-icon>
                <span>Invite bot</span>
            </v-btn>
            <v-btn text to="/commands">
                <v-icon left>mdi-view-list</v-icon>
                <span>Commands</span>
            </v-btn>
        </div>
        <v-spacer></v-spacer>
        <v-btn to="/manage" text>
            <span class="mr-2">Dashboard</span>
            <v-icon>mdi-login-variant</v-icon>
        </v-btn>
    </v-app-bar>
    <v-content>
    <router-view />
    <v-footer absolute class="px-12 py-3">
      <v-col
        cols="12"
      >
        &copy; {{ new Date().getFullYear() }} <a href="https://cubepotato.eu"><b>$Jakub$</b></a> & the <b>Lime Bot Team</b>
      </v-col>
    </v-footer>
</v-content>
</v-app>
</template>

<script>
export default {
    data: () => ({
        drawer: null
    })
}
</script>