import MapView from "react-native-maps";
import MapViewDirections from "react-native-maps-directions";
import {Text, View} from "react-native";
import React from "react";

const origin = {latitude: 51.4511732, longitude: -0.2138706};
const destination = {latitude: 51.5250836, longitude: -0.0769465};
const GOOGLE_MAPS_APIKEY = 'Some key';

export const  Map = () => {
    return(<>
        <MapView
            style={{flex: 1}}
            initialRegion={{
                latitude: 51.509864,
                longitude: -0.118092,
                latitudeDelta: 0.0922,
                longitudeDelta: 0.0421,
            }}>
            <MapViewDirections
                origin={origin}
                destination={destination}
                apikey={GOOGLE_MAPS_APIKEY}
            />
        </MapView>
        <View>
            <Text style={{flex: 0}}>Stuff</Text>
        </View>
        </>)
}