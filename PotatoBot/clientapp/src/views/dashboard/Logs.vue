<template>
    <v-container>
        <h1 class="display-1 mb-3">Logs</h1>
        <v-row>
            <v-col md="6">
                <v-row>
                    <v-col>
                        <h2>Moderation logs</h2>
                    </v-col>
                    <v-col cols="auto">
                        <switch-input id="enableModLogs" class="ma-0" v-model="modLogsEnabled"/>
                    </v-col>
                </v-row>

                <channel-input id="modLogsChannel" label="Moderation logs channel" :disabled="!modLogsEnabled" />

            </v-col>
        </v-row>
    </v-container>
</template>
<script>
import ChannelInput from '@/components/ChannelInput.vue';
import SwitchInput from '@/components/SwitchInput.vue';

export default {
    components: {ChannelInput, SwitchInput},
    data: ()=>({
        modLogsEnabled: false,
    })    
}
</script>