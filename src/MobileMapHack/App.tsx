import React from 'react';
import {StyleSheet, Text, View} from 'react-native';
import {MapView} from "expo";

export default function App() {

    return (
        <>
            <MapView
                style={{flex: 1}}
                initialRegion={{
                    latitude: 51.509865,
                    longitude: -0.118092,
                    latitudeDelta: 0.0922,
                    longitudeDelta: 0.0421,
                }}
            />
            <View>
                <Text style={{flex: 0}}>Stuff</Text>
            </View>
        </>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff',
        alignItems: 'center',
        justifyContent: 'center',
    },
});
