<template>
    <v-container>
        <h1 class="display-1 mb-3">Join / leave actions</h1>
        <v-row>
            <v-col md="6">
                <h2 class="mb-4">Autoroles</h2>
                <role-input id="autoRolesOnJoin" label="Roles to give when a member joins"/>

                <v-row class="mt-5">
                    <v-col>
                        <h2>Welcome & leave messages</h2>
                    </v-col>
                    <v-col cols="auto">
                        <switch-input id="enableWelcomeMessages" class="ma-0" v-model="welcomeMessagesEnabled"/>
                    </v-col>
                </v-row>
                

                <channel-input id="welcomeMessagesChannel" label="Channel" :disabled="!welcomeMessagesEnabled"/>

                <my-input 
                    id="welcomeMessage" 
                    label="Welcome message" 
                    :disabled="!welcomeMessagesEnabled" 
                    textarea maxlength="500" 
                    hint="Insert: {user} - mention/username, {members} - member count."
                    class="mb-4"/>
                <my-input 
                    id="leaveMessage" 
                    label="Leave message" 
                    :disabled="!welcomeMessagesEnabled" 
                    textarea maxlength="500"
                    hint="Insert: {user} - mention/username, {members} - member count."/>
            </v-col>
        </v-row>
    </v-container>
</template>
<script>
import MyInput from '@/components/MyInput.vue';
import RoleInput from '@/components/RoleInput.vue';
import SwitchInput from '@/components/SwitchInput.vue';
import ChannelInput from '@/components/ChannelInput.vue';

export default {
    components: {RoleInput, SwitchInput, ChannelInput, MyInput},
    data: ()=>({
        welcomeMessagesEnabled: false
    })
}
</script>