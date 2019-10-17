import React, {useEffect, useState, useContext} from 'react';
import {Card, CardItem, Content, Text, Body, View, Toast, Button, Icon} from "native-base";
import { FlatList, StyleSheet } from 'react-native';
import {BusLeg, JourneyAlternative, RouteInfo, WalkingLeg} from '../../domain/types';
import { api } from '../../api';
import JourneyContext from '../../context/JourneyContext';

export const JourneyPlannerAlternative = () => {

    const {journeyPlannerParams} = useContext(JourneyContext);
    const [alternativeJourney, setAlternativeJourney] = useState<JourneyAlternative>(null);
    const [showAlternativeJourney, toggleShowAlternativeJourney] = useState(false);

    const styles = StyleSheet.create({
        headerText: {
            fontWeight: '600'
        },
        content: {
            padding: 10
        },
        detailItem: {
            padding: 5
        }
    });

    useEffect(() => {
        // https://gladysint-insights-func.azurewebsites.net/api/JourneyOptions?code=Qa7a602gQhSdO8i4oCgAf5gv9flmxNUKCqyfa3rAakhwUOPiAuIkHw==&startDateTime=2019-10-04T10:00:00&startLongitude=${journeyPlannerParams.startLongitude}&startLatitude=${journeyPlannerParams.startLatitude}&endLongitude=${journeyPlannerParams.endLongitude}&endLatitude=${journeyPlannerParams.endLatitude}&mode=bus`;
        const url = `http://10.0.2.2:7071/api/JourneyOptions?startDateTime=${new Date().toISOString()}&startLongitude=${journeyPlannerParams.startLongitude}&startLatitude=${journeyPlannerParams.startLatitude}&endLongitude=${journeyPlannerParams.endLongitude}&endLatitude=${journeyPlannerParams.endLatitude}&mode=bus`;
        console.log('Calling api at: ' + url);

        api<JourneyAlternative>(url)
            .then(data => {
                console.log("*********** called api, setting journey planner data.");
                console.log(data.legs);
                setAlternativeJourney(data);
            })
            .catch(reason => {
                console.log(`***********  error calling map api: ${reason}`);
                // todo: toast does not show immediately
                Toast.show({
                    text: "There was a problem getting the route details",
                    position: "bottom",
                    type: "warning"
                });
            });
    }, [journeyPlannerParams]);

    const renderBusLeg = (leg: BusLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="bus" />
                <Text style={styles.headerText}>Bus</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Route: {leg.routeNumber}</Text>
                        <Text style={styles.detailItem}>Get on at : {leg.startPoint}</Text>
                        <Text style={styles.detailItem}>Get off at : {leg.finishPoint}</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderWalkingLeg = (leg: WalkingLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="walk" />
                <Text style={styles.headerText}>Walking</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Instruction: {leg.details}</Text>
                        <Text style={styles.detailItem}>Distance: {leg.distance} m</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    console.log("rendering")
    if (alternativeJourney == null){
        return null;
    } else {
        return (
        <Content style={styles.content}>
            <Card>
                <CardItem bordered style={{backgroundColor: 'red'}}>
                    <Text style={styles.headerText}>Journey Summary</Text>
                </CardItem>
                <CardItem style={{backgroundColor: '#eeeeee'}}>
                    <Body>
                        <View>
                            <Text style={styles.detailItem}>Distance: 12.3 miles</Text>
                            <Text style={styles.detailItem}>Travel Cost: £5.40</Text>
                            <Text style={styles.detailItem}>Travel time: 33 min</Text>
                        </View>
                    </Body>
                </CardItem>
                <CardItem button onPress={() => toggleShowAlternativeJourney(!showAlternativeJourney)}><Text>Alternative Journey</Text></CardItem>
            </Card>
            { showAlternativeJourney &&
            <FlatList
                data={alternativeJourney.legs}
                keyExtractor={(item, index) => index.toString()}
                renderItem={datum =>
                    datum.item.mode == 'bus' ? renderBusLeg(datum.item as BusLeg)
                        : (datum.item.mode == 'walking' ? renderWalkingLeg(datum.item as WalkingLeg)
                        : null) }
                /> }
        </Content>)
    }
};
