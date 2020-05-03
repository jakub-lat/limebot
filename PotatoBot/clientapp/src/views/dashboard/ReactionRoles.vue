<template>
    <v-container>
        <h1 class="display-1 mb-3">Reaction roles</h1>
        <v-hover v-slot:default="{ hover }">
          <v-card 
            color="grey darken-4" :elevation="hover ? 6 : 3" class="my-4" max-width="400" 
            link to="/commands/#reaction-roles"
          >
            <v-list-item>
                <v-list-item-content>
                    <v-list-item-title>Learn more about reaction roles</v-list-item-title>
                </v-list-item-content>

                <v-list-item-action>
                    <v-icon>mdi-arrow-right</v-icon>
                </v-list-item-action>
            </v-list-item>
          </v-card>
        </v-hover>
        <v-row class="mt-10">
            <v-col md="6">
                <v-row no-gutters>
                    <v-col>
                        Notify users on DM after granting/removing role? 
                    </v-col>
                    <v-col cols="auto">
                        <switch-input class="ma-0" id="reactionRolesNotifyDM" />
                    </v-col>
                </v-row>
            </v-col>
        </v-row>
    </v-container>
</template>
<script>
import SwitchInput from '@/components/SwitchInput.vue';
export default {
    components: {SwitchInput}
}
</script>