<template>
<v-app>
    <v-navigation-drawer app temporary v-model="drawer" class="d-sm-none">
        <v-list-item>
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
            <v-list-item link to="/#features">
            <v-list-item-action>
                <v-icon>mdi-palette</v-icon>
            </v-list-item-action>
            <v-list-item-content>
                <v-list-item-title>Features</v-list-item-title>
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
                <v-icon>mdi-home</v-icon>
                <span left>Start</span>
            </v-btn>
            <v-btn text to="/#features">
                <v-icon left>mdi-palette</v-icon>
                <span>Features</span>
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