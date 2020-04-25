<template>
    <v-container>
        <h1 class="display-1 mb-3">General settings</h1>
        <v-form>
            <v-row>
                <v-col md="5">
                    <my-input id="prefix" label="Prefix"/>
                </v-col>
            </v-row>
        </v-form>
    </v-container>
</template>
<script>
import MyInput from '@/components/MyInput.vue';
export default {
    components: {MyInput}
}
</script>