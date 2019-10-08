import MapView, { Marker, Polyline, PROVIDER_GOOGLE} from "react-native-maps";
//import MapViewDirections from "react-native-maps-directions";
import React, {useEffect, useRef, useState, useContext} from "react";
import {MapData, LatLng, Journey, JourneyAlternative} from "../../domain/types";
import {Button, Fab, Icon, Toast, View} from "native-base";
// @ts-ignore
import StartImg from "../../assets/start.png"
// @ts-ignore
import FinishImg from "../../assets/finish.png"
// @ts-ignore
import SchoolImg from "../../assets/placemark_circle.png"
// @ts-ignore
import OneImg from "../../assets/one.png"
// @ts-ignore
import TwoImg from "../../assets/two.png"
// @ts-ignore
import ThreeImg from "../../assets/three.png"
// @ts-ignore
import FourImg from "../../assets/four.png"
import {api} from "../../api"
import {fromNullable} from "fp-ts/lib/Option";
import JourneyContext from "../../context/JourneyContext";
import {SearchPanel} from "./searchPanel"
import {JourneyDetails} from "../../screens/route/journeyDetails";
import {useSlideInOutAnimation} from "../../hooks/animation";
import {Animated} from "react-native";
import {Marker as DomainMarker} from '../../domain/types';
import {JourneyPlannerAlternative} from "../../screens/route/JourneyPlannerAlternative";

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

export const Map = (props: any | {showSearch: boolean}) => {

    const [fabActive, setFabActive] = useState(() => false);
    const {journey, showPollution, showSchools, journeyPlannerParams, setJourney,
        togglePollution, toggleSchools, startTime, setJourneyPlannerParams} = useContext(JourneyContext);
    const [mapData, setMapData] = useState<MapData>();
    const mapRef = useRef<MapView>();
    const [selectedRouteIndex, setSelectedRouteIndex] = useState<number>(() => -1);
    const [showAlternative, setShowAlternative] = useState(false);
    const imgs = {
        "start": StartImg,
        "finish": FinishImg,
        "school": SchoolImg,
        "one": OneImg,
        "two": TwoImg,
        "three": ThreeImg,
        "four": FourImg
    };

    setJourney(props.navigation.getParam("journey") || journey);

    // should maybe based on map feature extents
    let region = fromNullable(journey)
        .map(j => calculateMapRegion(j))
        .getOrElse({centre: {latitude: 51.509864, longitude: -0.118092}, size: {latDelta: 0.0922, lonDelta: 0.0421}});

    let useCannedAlternativeJourneys = () => {
        useEffect(() => {
            console.log(`journey is ${journey}`);
            if (journey != null) {
                const url = `https://spectrummapapi.azurewebsites.net/api/map/mobile/${journey.id}?showPollution=${showPollution}&showSchools=${showSchools}&startTime=${startTime}`;
                console.log('Calling api at: ' + url);

                api<MapData>(url)
                    .then(data => {
                        console.log("*********** called api, setting map data.");
                        setMapData(data);
                        // not yet
                        // mapRef.current.fitToElements(true);
                    })
                    .catch(reason => {
                        console.log(`***********  error calling map api: ${reason}`);
                        // todo: can't have any other position that bottom does not show up
                        Toast.show({
                            text: "There was a problem getting the route details",
                            position: "bottom",
                            type: "warning"
                        });
                    });
            }
        }, [journey, showPollution, showSchools, startTime]);
    };

    // useCannedAlternativeJourneys();

    function getRouteStrokeWidth(defaultWidth: number, index:number): number {
        if (index === selectedRouteIndex) {
            return defaultWidth + 3;
        }
        else {
            return defaultWidth;
        }
    }

    function getMarkerOpacity(marker: DomainMarker) : number {
        // No selected route
        if (selectedRouteIndex === -1) {
            return 1.0;
        }

        // Selected route exists and marker intersects with selected route
        if (marker.intersectingRouteIndices != null &&
            marker.intersectingRouteIndices.includes(selectedRouteIndex)) {
                return 1.0;
        }

        // Selected route exists, but marker doesn't intersect
        return 0.5;
    }



    console.log("*********** rendering map");
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
                onMapReady={() => {
                    console.log("*********** map ready");
                }}
            >
                {mapData && mapData.lines.map((line, index) =>
                    <Polyline
                        key={"line" + index}
                        coordinates={line.coordinates}
                        strokeWidth={getRouteStrokeWidth(line.strokeWidth, index)}
                        strokeColor={line.strokeColor}
                        tappable={true}
                        onPress={() => setSelectedRouteIndex(index)}
                    />
                )}
                {mapData && mapData.markers.map((marker, index) =>
                    <Marker
                        key={"marker" + index}
                        title={marker.title}
                        image={imgs[marker.image]}
                        coordinate={marker.coordinates}
                        opacity={getMarkerOpacity(marker)}
                    />
                )}
            </MapView>
            <SearchPanel show={props.showSearch} journey={journey} />
            <Animated.View
                style={{
                    position: "absolute",
                    top: useSlideInOutAnimation(showAlternative, 50, 750),
                    bottom: 100,
                    left: 20,
                    right: 20 }}>
                <JourneyPlannerAlternative />
            </Animated.View>
            <Fab
                position="bottomLeft"
                style={{backgroundColor: 'red'}}
            >
                <Icon
                    name="stop"
                    type="MaterialIcons"
                    onPress={() => {
                        if (!showAlternative) {
                            setJourneyPlannerParams(null);
                        }
                        setShowAlternative(!showAlternative);
                    }}
                />
            </Fab>
            <Fab
                direction="up"
                position="bottomRight"
                active={fabActive}
                onPress={() => setFabActive(!fabActive)}>
                <Icon name="settings" type="MaterialIcons"/>
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
