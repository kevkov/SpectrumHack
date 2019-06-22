import {Animated} from "react-native";
import MapView, { Marker, Polyline, PROVIDER_GOOGLE} from "react-native-maps";
//import MapViewDirections from "react-native-maps-directions";
import React, {useEffect, useRef, useState, useContext} from "react";
import {MapData, LatLng, Journey} from "../../domain/types";
import {Button, Fab, Icon, Input, Card, Toast, CardItem, Label, Picker, View} from "native-base";
// @ts-ignore
import StartImg from "../../assets/start.png"
// @ts-ignore
import FinishImg from "../../assets/finish.png"
// @ts-ignore
import SchoolImg from "../../assets/school.png"
// @ts-ignore
import OneImg from "../../assets/one.png"
// @ts-ignore
import TwoImg from "../../assets/two.png"
// @ts-ignore
import ThreeImg from "../../assets/three.png"
// @ts-ignore
import FourImg from "../../assets/four.png"
import {api} from "../../api"
import { fromNullable } from "fp-ts/lib/Option";
import JourneyContext from "../../context/JourneyContext";

const GOOGLE_MAPS_APIKEY = '';

function calculateMapRegion(journey: Journey): { centre:LatLng, size: {latDelta: number, lonDelta: number}} {
    let maxLoc = {
        latitude: Math.max(journey.start.latitude, journey.end.latitude),
        longitude: Math.max(journey.start.longitude, journey.end.longitude)
    };
    let minLoc = {
        latitude: Math.min(journey.start.latitude, journey.end.latitude),
        longitude: Math.min(journey.start.longitude, journey.end.longitude)
    };
    let latDelta = maxLoc.latitude - minLoc.latitude;
    let lonDelta = maxLoc.longitude - minLoc.longitude;
    let centre = {latitude: minLoc.latitude + (0.5 * latDelta), longitude: minLoc.longitude + (0.5 * lonDelta)};
    return { centre, size: {latDelta, lonDelta} };
}

export const Map = (props) => {
    let journey: Journey | null = props.navigation.getParam("journey");

    // should maybe based on map feature extents
    let region = fromNullable(journey)
        .map(j => calculateMapRegion(j))
        .getOrElse({centre: {latitude: 51.509864, longitude: -0.118092}, size: {latDelta: 0.0922, lonDelta: 0.0421}});

    const [fabActive, setFabActive] = useState(() => false);
    const {showPollution, showSchools, togglePollution, toggleSchools, startTime} = useContext(JourneyContext);
    const [mapData, setMapData] = useState<MapData>();
    const mapRef = useRef<MapView>();
    const [showSearch, toggleSearch] = useState(() => false);

    const imgs = {
        "start": StartImg,
        "finish": FinishImg,
        "school": SchoolImg,
        "one": OneImg,
        "two": TwoImg,
        "three": ThreeImg,
        "four": FourImg
    };

    useEffect(() => {
        if (journey != null) {
            const url = `https://spectrummapapi.azurewebsites.net/api/map/mobile/${journey.id}?showPollution=${showPollution}&showSchools=${showSchools}&startTime=${startTime}`;
            console.log('Calling api at: ' + url);

            api<MapData>(url)
                .then(data => {
                    console.log("*********** called api, setting map data." );
                    setMapData(data);
                    // not yet
                    // mapRef.current.fitToElements(true);
                })
                .catch(reason => {
                    console.log(`***********  error calling map api: ${reason}`);
                    // todo: can't have any other position that bottom does not show up
                    Toast.show({text: "There was a problem getting the route details", position: "bottom", type: "warning"});
                });
        }
    }, [journey, showPollution, showSchools]);

    console.log("*********** rendering");
    return (
        <View style={{flex: 1}}>
            <MapView
                ref={mapRef}
                provider={PROVIDER_GOOGLE}
                style={{flex: 1 }}
                initialRegion={{
                    latitude: region.centre.latitude,
                    longitude: region.centre.longitude,
                    latitudeDelta: 1.05 * region.size.latDelta,
                    longitudeDelta: 1.05 * region.size.lonDelta
                }}
                onPress={() => toggleSearch(!showSearch)}
                onMapReady={() => {
                    console.log("*********** map ready");
                }}
            >
                {mapData && mapData.lines.map((line, index) =>
                    <Polyline
                        key={"line" + index}
                        coordinates={line.coordinates}
                        strokeWidth={line.strokeWidth}
                        strokeColor={line.strokeColor}
                    />
                )}
                {mapData && mapData.markers.map((marker, index) =>
                    <Marker
                        key={"marker" + index}
                        title={marker.title}
                        image={imgs[marker.image]}
                        coordinate={marker.coordinates}
                    />
                )}
            </MapView>
            <SearchPanel show={showSearch} />
            <Fab
                direction="up"
                position="bottomRight"
                active={fabActive}
                onPress={() => setFabActive(!fabActive)}>
                <Icon name="playlist-add-check" type="MaterialIcons"/>
                <Button
                    onPress={() => togglePollution(!showPollution)}
                    style={{backgroundColor: showPollution ? "#B5651D" : "#CCCCCC"}}>
                    <Icon name="cloud-circle" type="MaterialIcons"/>
                </Button>
                <Button
                    onPress={() => toggleSchools(!showSchools)}
                    style={{backgroundColor: showSchools ? "#397D02" : "#CCCCCC"}}>
                    <Icon name="school" type="MaterialIcons"/>
                </Button>
            </Fab>
        </View>)
};

const SearchPanel = (props:{show:boolean}) => {

    const {show} = props;
    const [visible, setVisible] = useState(false);

    let startTop = new Animated.Value(-250);
    let endTop = 10;
    let top = (visible ? endTop : startTop);

    useEffect(() => {
        if (show == true && visible === false) {
            Animated.timing(
                startTop,
                {
                    toValue: endTop,
                    duration: 500,
                }
            ).start(() => setVisible(true));
        }
    });

    if (show === false)
    {
        if (visible !== false) setVisible(false);
        return null;
    }

    const range = new Array(47).fill(0);
    return (
        <Animated.View style={{top: top, position: 'absolute', right: 10, left: 10}}>
        <Card style={{borderRadius: 5}}>
            <CardItem>
                <Input  placeholder="From" style={{flex: 4, borderWidth: 1, borderRadius: 5, borderColor: "#CCCCCC"}}/>
            </CardItem>
            <CardItem>
                <Input placeholder="To" style={{flex: 4, borderWidth: 1, borderRadius: 5, borderColor: "#CCCCCC"}}/>
            </CardItem>
            <CardItem>
                <Label style={{marginRight: 5}}>Time</Label>
                <Picker mode="dropdown">
                    {range.map((_, index) => {
                        const hour = Math.floor(index / 2);
                        const mins = index % 2 == 0 ? "00" : "30";
                        const time = `${hour < 10 ? "0" + hour : hour.toString()}:${mins}`;
                        console.log(time)
                        return (<Picker.Item label={time} value={time} key={time}/>);
                    })}
                </Picker>
                <Button primary style={{width:50, height:50, borderRadius:25, alignItems:"center", justifyContent:"center"}}>
                    <Icon name="search" type="MaterialIcons" />
                </Button>
            </CardItem>
        </Card>
        </Animated.View>
    )
};
