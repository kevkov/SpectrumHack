import MapView, {KmlMapEvent, PROVIDER_GOOGLE} from "react-native-maps";
//import MapViewDirections from "react-native-maps-directions";
import {View} from "react-native";
import React, {useState} from "react";
import {Location} from "../../domain/types";
import {Button, Fab, Icon } from "native-base";

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

    const [fabActive, setFabActive] = useState(() => false);
    const [showPollution, togglePollution] = useState(() => true);
    const [showSchools, toggleSchools] = useState(() => true);

    return (<View style={{flex:1}}>
                <MapView
                    provider={PROVIDER_GOOGLE}
                    style={{flex: 1}}
                    initialRegion={{
                        latitude: centre.latitude,
                        longitude: centre.longitude,
                        latitudeDelta: 1.05 * latDelta,
                        longitudeDelta: 1.05 * lonDelta }}
                    />
                <Fab
                    direction="up"
                    position="bottomRight"
                    active={fabActive}
                    onPress={() => setFabActive(!fabActive)}>
                    <Icon name="playlist-add-check" type="MaterialIcons"/>
                    <Button
                        onPress={() => togglePollution(!showPollution)}
                        style={{backgroundColor: showPollution ? "#B5651D" : "#CCCCCC"}}>
                        <Icon name="cloud-circle" type="MaterialIcons" />
                    </Button>
                    <Button
                        onPress={() => toggleSchools(!showSchools)}
                        style={{backgroundColor: showSchools ? "#397D02" : "#CCCCCC"}}>
                        <Icon name="school" type="MaterialIcons" />
                    </Button>
                </Fab>
            </View>)
};
