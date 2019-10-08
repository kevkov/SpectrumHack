import React, {useEffect, useState, useContext} from 'react';
import {Card, CardItem, Content, Text, Body, View, Toast, Button} from "native-base";
import { FlatList, StyleSheet } from 'react-native';
import {JourneyAlternative, RouteInfo} from '../../domain/types';
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
        const url = `http://10.0.2.2:7071/api/OptionalJourney?startDateTime=2019-10-04T10:00:00&startLongitude=0.1858&startLatitude=51.5751&endLongitude=-0.118092&endLatitude=51.509865`;
        console.log('Calling api at: ' + url);

        api<JourneyAlternative>(url)
            .then(data => {
                console.log("*********** called api, setting journey planner data.");
                console.log(data.legs);
                setAlternativeJourney(data);
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
    }, [journeyPlannerParams]);


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
                            <Text style={styles.detailItem}>Green score: 12</Text>
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
                    <Card>
                        <CardItem bordered>
                            <Text style={styles.headerText}>datum</Text>
                        </CardItem>
                        <CardItem style={{backgroundColor: '#eeeeee'}}>
                            <Body>
                                <View>
                                    <Text style={styles.detailItem}>Green score: </Text>
                                    <Text style={styles.detailItem}>Schools: </Text>
                                    <Text style={styles.detailItem}>Distance: miles</Text>
                                    <Text style={styles.detailItem}>Average Air Quality: </Text>
                                    <Text style={styles.detailItem}>Travel time: </Text>
                                    <Text style={styles.detailItem}>Travel cost: </Text>
                                </View>
                            </Body>
                        </CardItem>
                    </Card>
                }/> }
        </Content>)
    }
};
