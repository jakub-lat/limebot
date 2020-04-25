<template>
    <v-container>
        <h1 class="display-1 mb-3">General settings</h1>
        <v-row>
            <v-col md="6">
                <my-input id="prefix" label="Prefix (default: $)"/>
            </v-col>
        </v-row>
    </v-container>
</template>
<script>
import MyInput from '@/components/MyInput.vue';
export default {
    components: {MyInput}
}
</script>