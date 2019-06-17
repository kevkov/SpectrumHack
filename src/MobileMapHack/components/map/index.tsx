import MapView, {PROVIDER_GOOGLE} from "react-native-maps";
//import MapViewDirections from "react-native-maps-directions";
import {Text, View} from "react-native";
import React from "react";
import {Location} from "../../domain/types";

const GOOGLE_MAPS_APIKEY = '';

export const Map = (props) => {
    let origin: Location = props.navigation.getParam("origin");
    let dest: Location = props.navigation.getParam("destination");
    let maxLoc = {
        latitude: Math.max(origin.latitude, dest.latitude),
        longitude: Math.max(origin.longitude, dest.longitude)
    };
    let minLoc = {
        latitude: Math.min(origin.latitude, dest.latitude),
        longitude: Math.min(origin.longitude, dest.longitude)
    };
    let latDelta = maxLoc.latitude - minLoc.latitude;
    let lonDelta = maxLoc.longitude - minLoc.longitude;
    let centre = {latitude: minLoc.latitude + (0.5 * latDelta), longitude: minLoc.longitude + (0.5 * lonDelta)};
    return (<>
                <MapView
                    provider={PROVIDER_GOOGLE}
                    style={{flex: 1}}
                    region={{
                        latitude: centre.latitude,
                        longitude: centre.longitude,
                        latitudeDelta: 1.05 * latDelta,
                        longitudeDelta: 1.05 * lonDelta }}
                    kmlSrc={"https://spectrummapapi.azurewebsites.net/api/map"} />
                <View>
                    <Text style={{flex: 0}}>Stuff</Text>
                </View>
            </>)
};
